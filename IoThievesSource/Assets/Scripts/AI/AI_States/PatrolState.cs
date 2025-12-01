using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class PatrolState : AIState
    {
         private float _speed = 2.0f;
         private float _angularSpeed = 2f;
         private bool _isMoving = true;
         private bool _isInteracting = false;
        
         private Waypoint _target;
         private Waypoint _lastWaypoint;
         private float _threshold = 0.8f; 
         private float _angularThreshold = 45.0f; //degrees

        
        private Queue<Movement> _recentMovements = new Queue<Movement>();
        private const int MaxMovementHistory = 15;


        public PatrolState(AIController npc) : base(npc){}

        public Waypoint ChooseNextWaypoint(Waypoint currentWaypoint)
        {
            if (currentWaypoint.neighbours == null || currentWaypoint.neighbours.Count == 0)
                return null;

            List<Waypoint> availableWaypoints = new List<Waypoint>();

            foreach (Waypoint neighbour in currentWaypoint.neighbours)
            {
                if (!IsInRecentMovements(currentWaypoint, neighbour))
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
                       mask))
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
            
            float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
            float angleMag = Mathf.Abs(angle);
            
            _isMoving = angleMag < _angularThreshold;

            float rotateOf = angle * _angularSpeed * Time.deltaTime;
            Npc.transform.Rotate(0,rotateOf,0);

            if (_isMoving)
            {
                Vector3 velocity = dir * (_speed * Time.deltaTime);
                Npc.transform.position  += new Vector3(velocity.x, 0, velocity.z);
            }

            float distance = (_target.transform.position - Npc.transform.position).magnitude;
            
            if (distance < _threshold)
            {
                if (_target is PointOfInterest poi)
                {
                    _isMoving = false;
                    _isInteracting = !poi.AssignedDevice.Interact();
                }

                if (!_isInteracting)
                {
                    _target = ChooseNextWaypoint(_target);
                    _isMoving = true;
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
