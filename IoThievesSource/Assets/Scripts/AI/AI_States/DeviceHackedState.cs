using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class DeviceHackedState : AIState
    {
        private float speed = 2.0f;
        private float angularSpeed = 2f;
        
        private PointOfInterest hackedDevice;
        private List<Waypoint> path;
        private bool destinationReached = false;

        private float turningThreshold = 1.0f;
        private float nextWaypointThreshold = 0.5f;

        private bool isResolving = false;
        
        public DeviceHackedState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            PointOfInterest[] hackedObjects = Object.FindObjectsByType<PointOfInterest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (PointOfInterest poi in hackedObjects)
            {
                if (!(poi.AssignedDevice is null))
                    hackedDevice = poi;
            }
            if(hackedDevice is not null)
                path = PathFinding.FindPath(findNearestWaypoint(), hackedDevice);
        }

        private Waypoint findNearestWaypoint()
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
            if (destinationReached)     //Resolve the reached device
            {
                isResolving = hackedDevice.interact();

                if (!isResolving)
                {
                    //Get all the hacked device
                    //remove the current solved device
                    //get the next device
                    //compute the new path
                    //path = PathFinding.FindPath(findNearestWaypoint(), hackedDevice);
                    destinationReached = false;
                    //If no more hacked device --> Npc.ChangeState
                }
            }
            else                  //Reach the hacked device
            {
                
                Vector3 dir = path[0].transform.position - Npc.transform.position;
                Vector3 velocity = dir.normalized * (speed * Time.deltaTime);
                Npc.transform.position  += new Vector3(velocity.x, 0, velocity.z);

                if (dir.magnitude < turningThreshold)
                {
                    Vector3 forwardDir = Npc.transform.forward.normalized;
                    float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
                    float rotateOf = angle * angularSpeed * Time.deltaTime;
                    Npc.transform.Rotate(0,rotateOf,0);
                }
            
                float distance = Vector3.Distance(Npc.transform.position, path[0].transform.position);
                if (distance < nextWaypointThreshold)
                {
                    Waypoint reached = path[0];
                    path.RemoveAt(0);

                    if (reached.Equals(hackedDevice))
                    {
                        destinationReached = true;
                    }
                }
            }
        }

        public override void Exit()
        {
        }
    }
}