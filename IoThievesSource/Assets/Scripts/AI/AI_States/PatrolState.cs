using System;
using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.AI_States
{
    public class PatrolState : AIState
    {
         private bool _isMoving = false; 
        
         private bool _isInteracting = false;
        
         private Waypoint _target;
         private Waypoint _lastWaypoint;
         
        private Queue<Movement> _recentMovements = new Queue<Movement>();
        private const int MaxMovementHistory = 15;

        private bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (value == _isMoving) return;

                _isMoving = value;
                RaiseOnWalkingVarChanged(_isMoving); 
            }
        }
        
        public PatrolState(AIController npc) : base(npc){}

        public Waypoint ChooseNextWaypoint(Waypoint currentWaypoint)
        {
            if (currentWaypoint.neighbours == null || currentWaypoint.neighbours.Count == 0)
                return null;

            List<Waypoint> availableWaypoints = new List<Waypoint>();

            foreach (Waypoint neighbour in currentWaypoint.neighbours)
            {
                if (!IsInRecentMovements(currentWaypoint, neighbour) && IsAvailable(currentWaypoint, neighbour)) 
                {
                    availableWaypoints.Add(neighbour);
                }
            }

            if (availableWaypoints.Count == 0)
            {
                availableWaypoints.AddRange(currentWaypoint.neighbours);
            }

            Waypoint nextWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Count)];

            AddMovement(currentWaypoint, nextWaypoint);

            return nextWaypoint;
        }

        private bool IsAvailable(Waypoint currentWaypoint, Waypoint destination)
        {
            bool isWalkable;
            try
            {
                isWalkable = Npc.WalkableArea.IsWalkable(destination);
            }
            catch (Exception _)
            {
                isWalkable = true;
            }

            return isWalkable
                   && !Physics.Raycast(currentWaypoint.transform.position,
                       (destination.transform.position - currentWaypoint.transform.position).normalized,
                       Vector3.Distance(currentWaypoint.transform.position, destination.transform.position),
                       LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore)
                   && TemporaryUnaccessibleAreasManager.IsAccessible(destination);

        }

        private void AddMovement(Waypoint from, Waypoint to)
        {
            // Rimuove il movimento più vecchio se abbiamo raggiunto il limite
            if (_recentMovements.Count >= MaxMovementHistory)
            {
                _recentMovements.Dequeue();
            }

            _recentMovements.Enqueue(new Movement(from, to));
        } 
        
        private bool IsInRecentMovements(Waypoint from, Waypoint to)
        {
            foreach (Movement movement in _recentMovements)
            {
                if (movement.Equals(from, to))
                    return true;
            }
            return false;
        }

        public override void Enter() //Chooses the First waypoint to reach
        {
            LayerMask mask = LayerMask.GetMask("Default");
            if (!(_lastWaypoint is null))        
            {
                Vector3 direction = _lastWaypoint.transform.position - Npc.transform.position;
                float distance = direction.magnitude;
                
                if (!Physics.Raycast(
                        Npc.transform.position,
                        direction.normalized,
                        distance,
                        mask))           //If the last targeted destination is still reachable
                {
                    _target = _lastWaypoint;
                    return;
                }
            }

            
            Waypoint[] wps = UnityEngine.Object.FindObjectsByType<Waypoint>(
               FindObjectsInactive.Exclude,
               FindObjectsSortMode.None);
           
            float maxCheckDistance = 50f;
            float bestDistance = Mathf.Infinity;
            Waypoint bestWaypoint = null;
           
            foreach (Waypoint wp in wps)
            { 
                Vector3 dir = wp.transform.position - Npc.transform.position; 
                float distance = dir.magnitude;

                if (distance > maxCheckDistance)
                   continue; 
               
                if (!Physics.Raycast(
                       Npc.transform.position,
                       dir.normalized,
                       distance,
                       mask, QueryTriggerInteraction.Ignore))
                { 
                    if (distance < bestDistance)
                    {
                       bestDistance = distance;
                       bestWaypoint = wp;
                    }
                }
            }
            _target = bestWaypoint;
        }

        public override void Execute()
        {
            if (_target is null)
                return;
            
            Vector3 dir = _target.transform.position - Npc.transform.position;
            dir.Normalize();

            Vector3 forwardDir = Npc.transform.forward.normalized;

            float angleMag = AIUtilities.RotateTarget(Npc.transform, forwardDir, dir, Npc.NpcDatas.AngularSpeed); 
            
            IsMoving = angleMag < Npc.NpcDatas.AngularThreshold;
            Npc.enemyAdapter.AnimationController.SetWalking(IsMoving);

            if (IsMoving)
            {
                AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed);
            }

            float distance = (_target.transform.position - Npc.transform.position).magnitude;
            
            if (distance < Npc.NpcDatas.DistanceThreshold)
            { 
                /*
                if (_target is PointOfInterest poi)
                {
                    IsMoving = false;
                    _isInteracting = !poi.AssignedDevice.Interact();
                }
                */
                
                if (!_isInteracting)
                {
                    _target = ChooseNextWaypoint(_target);
                    IsMoving = true;
                }
            }
        }

        public override void Exit()
        {
            _lastWaypoint = _target;
        }
    }
    
    struct Movement
    {
        public Waypoint From;
        public Waypoint To;

        public Movement(Waypoint from, Waypoint to)
        {
            this.From = from;
            this.To = to;
        }

        public bool Equals(Waypoint fromWaypoint, Waypoint toWaypoint)
        {
            return From == fromWaypoint && To == toWaypoint;
        }
    }
}
