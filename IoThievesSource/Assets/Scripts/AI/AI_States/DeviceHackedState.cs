using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class DeviceHackedState : AIState
    {
        private float _speed = 2.0f;
        private float _angularSpeed = 2f;
        
        private PointOfInterest _hackedDevice;
        private List<Waypoint> _path;
        private bool _destinationReached = false;

        private float _turningThreshold = 1.0f;
        private float _nextWaypointThreshold = 0.5f;

        private bool _isResolving = false;
        
        public DeviceHackedState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            PointOfInterest[] hackedObjects = Object.FindObjectsByType<PointOfInterest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (PointOfInterest poi in hackedObjects)
            {
                if (!(poi.AssignedDevice is null))
                    _hackedDevice = poi;
            }
            if(_hackedDevice is not null)
                _path = PathFinding.FindPath(FindNearestWaypoint(), _hackedDevice);
        }

        private Waypoint FindNearestWaypoint()
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
            
            return bestWaypoint;
        }
        
        public override void Execute()
        {
            if (_destinationReached)     //Resolve the reached device
            {
                _isResolving = _hackedDevice.interact();

                if (!_isResolving)
                {
                    //Get all the hacked device
                    //remove the current solved device
                    //get the next device
                    //compute the new path
                    //path = PathFinding.FindPath(findNearestWaypoint(), hackedDevice);
                    _destinationReached = false;
                    //If no more hacked device --> Npc.ChangeState
                }
            }
            else                  //Reach the hacked device
            {
                
                Vector3 dir = _path[0].transform.position - Npc.transform.position;
                Vector3 velocity = dir.normalized * (_speed * Time.deltaTime);
                Npc.transform.position  += new Vector3(velocity.x, 0, velocity.z);

                if (dir.magnitude < _turningThreshold)
                {
                    Vector3 forwardDir = Npc.transform.forward.normalized;
                    float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
                    float rotateOf = angle * _angularSpeed * Time.deltaTime;
                    Npc.transform.Rotate(0,rotateOf,0);
                }
            
                float distance = Vector3.Distance(Npc.transform.position, _path[0].transform.position);
                if (distance < _nextWaypointThreshold)
                {
                    Waypoint reached = _path[0];
                    _path.RemoveAt(0);

                    if (reached.Equals(_hackedDevice))
                    {
                        _destinationReached = true;
                    }
                }
            }
        }

        public override void Exit()
        {
        }
    }
}