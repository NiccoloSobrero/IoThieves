using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.AI_States
{
    public class SuspiciousState : AIState
    {
        private float _speed = 1.50f;
        private float _angularSpeed = 2f;
        private bool _isMoving = true;
        
        private Waypoint _destinationWaypoint;
        
        private Vector3 _nextDestination;
        
        private float _threshold = 0.3f; 
        private float _angularThreshold = 30.0f; //degrees
        private float _minDistance = 1.5f;        //Below this distance no intermediate points will be added
        private Vector3[] _positions;
        private int _positionIndex = 0;
        private float _radius = 0.5f;

        private float _waitedTime = 0;
        private float _waitingTime;
        private const float MaxWaitingTime = 3.0f;
        private bool _midpointReached = false;
        
        private Queue<Waypoint> _recentMovements = new Queue<Waypoint>();
        private const int MaxMovementHistory = 15;
        public SuspiciousState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            //Text : What's that -> Looking around animation??
            LayerMask mask = LayerMask.GetMask("Default");
            
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
            _destinationWaypoint = bestWaypoint;
            ComputeMiddlePoints();
            
            //Wait a second before it starts moving
            _midpointReached = true;
            _isMoving = false;
            _waitingTime = Random.value * MaxWaitingTime; //Random waiting time up to 3 seconds
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
                    _isMoving = true;
                }
            }
            else
            {
                Vector3 dir = _nextDestination - Npc.transform.position;
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

                float distance = (_nextDestination - Npc.transform.position).magnitude;
            
                if (distance < _threshold)
                {
                    _midpointReached = true;
                    _isMoving = false;
                    _waitingTime = Random.value * MaxWaitingTime; //Random waiting time up to 3 seconds
                }
            }
        }

        public override void Exit()
        {
          //Text: Must have been the wind 
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
            { //If I reached a Waypoint ---> New Waypoint is Needed
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
           int points = Random.Range(0,Mathf.FloorToInt(distance / _minDistance)+1);
           _positions = new Vector3[points];

           for (int i = 0; i < points; i++)
           {
               float t = (float)(i + 1) / (points + 1);

               float angle = Random.Range(0f, Mathf.PI * 2f);
               float x = Mathf.Cos(angle) * _radius;
               float z = Mathf.Sin(angle) * _radius;

               _positions[i] = Vector3.Lerp(Npc.transform.position, _destinationWaypoint.transform.position, t) +
                              new Vector3(x, .1f, z);
           }

           _positionIndex = 0;
           NextDestination(); 
        }
        
    }
    
}