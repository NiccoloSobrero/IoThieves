using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class NoiseHeardState : AIState
    {
        private float _speed = 2.0f;
        private float _angularSpeed = 2f;
        
        private Transform _throwedObject;
        private List<Waypoint> _path;
        private bool _destinationReached = false;

        private float _turningThreshold = 1.0f;
        private float _nextWaypointThreshold = 0.5f;
        private float _throwedObjectSurroundings = 0.3f;

        public NoiseHeardState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            _throwedObject = null; //Some way to retrieve this info
            Waypoint nearestObj = FindNearestWaypoint(_throwedObject.position);
            Waypoint nearestWaypoint = FindNearestWaypoint(Npc.transform.position);
            
            _path = PathFinding.FindPath(nearestWaypoint, nearestObj);
            _destinationReached = false;
        }

        public override void Execute()
        {
            if (_destinationReached)     
            {
                //lookAround animation
                Debug.Log("Destination Reached");
                Npc.PreviousState();
            }
            else                  //Reach the throwed object 
            {
                Transform destination = (_path.Count > 0) ? _path[0].transform : _throwedObject;
                
                Vector3 dir = destination.position - Npc.transform.position;
                Vector3 velocity = dir.normalized * (_speed * Time.deltaTime);
                Npc.transform.position  += new Vector3(velocity.x, 0, velocity.z);
                
                Vector3 forwardDir = Npc.transform.forward.normalized;
                Vector3 lookDir = dir;
                
                if (dir.magnitude < _turningThreshold && _path.Count > 1)
                   lookDir = _path[1].transform.position - Npc.transform.position;
                    
                float angle = Vector3.SignedAngle(forwardDir, lookDir, Vector3.up);
                float rotateOf = angle * _angularSpeed * Time.deltaTime;
                Npc.transform.Rotate(0,rotateOf,0);
            
                float distance = Vector3.Distance(Npc.transform.position, destination.position);
                if (distance < _nextWaypointThreshold)
                {
                    Transform reached = destination;
                    if (_path.Count > 0)
                    {
                        _path.RemoveAt(0);
                    }

                    if (Vector3.Distance(reached.position , _throwedObject.position) < _throwedObjectSurroundings && _path.Count == 0)
                    {
                        _destinationReached = true;
                    }
                }
            }

        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
        
        private Waypoint FindNearestWaypoint(Vector3 position)
        {
            LayerMask mask = LayerMask.GetMask("Default");
            
            Waypoint[] wps = Object.FindObjectsByType<Waypoint>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);
           
            float maxCheckDistance = 10f;
            float bestDistance = Mathf.Infinity;
            Waypoint bestWaypoint = null;
           
            foreach (Waypoint wp in wps)
            { 
                Vector3 dir = wp.transform.position - position; 
                float distance = dir.magnitude;

                if (distance > maxCheckDistance)
                    continue; 
               
                if (!Physics.Raycast(
                        position,
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
            
            return bestWaypoint;
        }
    }
    
}
