using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.AI_States
{
    public class SuspiciousState : AIState
    {
        private bool _verySuspicious = false;
        
        private float _speed = 1.50f;
        private float _angularSpeed = 2f;
        private bool _isMoving = false;
        
        private Waypoint _destinationWaypoint;
        private Vector3 _nextDestination;

        private const float MinDistance = 1.5f;        //Below this distance no intermediate points will be added
        private const float Radius = 0.3f;
        private Vector3[] _positions;
        private int _positionIndex = 0;

        private float _waitedTime = 0;
        private float _waitingTime;
        private const float MaxWaitingTime = 3.0f;
        private bool _midpointReached = false;
        
        private Queue<Waypoint> _recentMovements = new();
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
        
        public SuspiciousState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            _destinationWaypoint = AIUtilities.FindNearestWaypoint(Npc.transform.position);
            ComputeMiddlePoints();
            
            _midpointReached = true;
            
            _waitingTime = Mathf.Max(Random.value * MaxWaitingTime, 0.1f); //Random waiting time up to 3 seconds
        }
        
        public override void Execute()
        {
            if (_destinationWaypoint is null)
                return;

            if (_midpointReached)               //Wait before continue searching
            {
                _waitedTime += (1.0f) / _waitingTime * Time.deltaTime;
                if (_waitedTime >= 1.0f)
                {
                    _midpointReached = false;
                    _waitedTime = 0;
                    NextDestination();
                    IsMoving = true;
                }
            }
            else
            {
                Vector3 dir = _nextDestination - Npc.transform.position;
                if (dir.sqrMagnitude < 0.01f)
                {
                    _midpointReached = true;
                    IsMoving = false;
                    return;
                }
                
                dir.Normalize();

                Vector3 forwardDir = Npc.transform.forward.normalized;

                float angleMag = AIUtilities.RotateTarget(Npc.transform, forwardDir, dir, _angularSpeed); 
                
                IsMoving = angleMag < Npc.NpcDatas.AngularThreshold;

                if (IsMoving)
                { 
                    AIUtilities.MoveTarget(Npc.transform, dir, _speed); 
                }

                float distance = (_nextDestination - Npc.transform.position).magnitude;
            
                if (distance < Npc.NpcDatas.DistanceThreshold)
                {
                    _midpointReached = true;
                    IsMoving = false;
                    _waitingTime = Random.value * MaxWaitingTime * (_verySuspicious ? 0.5f : 1.0f); //Random waiting time up to 3 seconds (halved if verySuspicious)
                }
            }
        }

        public override void Exit()
        {
            _verySuspicious = false;
        }

        public void VerySuspicious(bool suspicious)
        {
            if (suspicious)
            {
                _speed = Npc.NpcDatas.AlternativeSpeed;
                _angularSpeed = Npc.NpcDatas.AlternativeAngularSpeed;
                _verySuspicious = true;
            }
            else
            {
                _speed = Npc.NpcDatas.Speed + 0.5f;
                _angularSpeed = Npc.NpcDatas.AngularSpeed + 0.5f;
                _verySuspicious = false;
            }
        }
        public void ChooseNextWaypoint(Waypoint currentWaypoint)
        {
            if (currentWaypoint.neighbours == null || currentWaypoint.neighbours.Count == 0)
                return;

            List<Waypoint> availableWaypoints = new List<Waypoint>();

            foreach (Waypoint neighbour in currentWaypoint.neighbours)
            {
                if (!IsInRecentMovements(neighbour))
                {
                    availableWaypoints.Add(neighbour);
                }
            }

            if (availableWaypoints.Count == 0)
            {
                availableWaypoints.AddRange(currentWaypoint.neighbours);
            }

            Waypoint nextWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Count)];

            AddMovement(nextWaypoint);

            _destinationWaypoint = nextWaypoint;
            ComputeMiddlePoints();
        }

        private void AddMovement(Waypoint to)
        {
            if (_recentMovements.Count >= MaxMovementHistory)
            {
                _recentMovements.Dequeue();
            }

            _recentMovements.Enqueue(to);
        } 
      
        private bool IsInRecentMovements(Waypoint to)
        {
            foreach (Waypoint movement in _recentMovements)
            {
                if (movement.Equals(to))
                    return true;
            }
            return false;
        }

        private void NextDestination()
        {
            if (Vector3.Distance(_nextDestination, _destinationWaypoint.transform.position) < 0.05f)
            { 
                //If I reached a Waypoint ---> New Waypoint is Needed
                ChooseNextWaypoint(_destinationWaypoint);
            }      
            else if (_positionIndex >= _positions.Length-1)
            {
                //I finished the middlePoints --> I have to reach the DestinationWayPoint
                _nextDestination = _destinationWaypoint.transform.position;
            }
            else
            {
                _nextDestination = _positions[_positionIndex];
                _positionIndex++;
            }
        }

        private void ComputeMiddlePoints()
        {
           float distance =  Vector3.Distance(Npc.transform.position, _destinationWaypoint.transform.position);
           int points = Random.Range(0,Mathf.FloorToInt(distance / MinDistance)+1);
           _positions = new Vector3[points];

           for (int i = 0; i < points; i++)
           {
               float t = (float)(i + 1) / (points + 1);

               float angle = Random.Range(0f, Mathf.PI * 2f);
               float x = Mathf.Cos(angle) * Radius;
               float z = Mathf.Sin(angle) * Radius;

               _positions[i] = Vector3.Lerp(Npc.transform.position, _destinationWaypoint.transform.position, t) +
                              new Vector3(x, .1f, z);
           }

           _positionIndex = 0;
           NextDestination(); 
        }
    }
}