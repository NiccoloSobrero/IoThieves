using System.Collections.Generic;
using System.Net.NetworkInformation;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class DeviceHackedState : AIState
    {
        
        private PointOfInterest _hackedDevice;
        private List<Waypoint> _path;
        private bool _destinationReached = false;

        private float _nextWaypointThreshold = 0.5f;

        private bool _solved = false;
        
        public DeviceHackedState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            _hackedDevice = FindHackedDevice();

            if (_hackedDevice is not null)
            {
                _path = PathFinding.FindPath(AIUtilities.FindNearestWaypoint(Npc.transform.position), _hackedDevice);
                _destinationReached = false;
            }
            else
                Npc.PreviousState();
        }

        private PointOfInterest FindHackedDevice()
        {
            PointOfInterest[] hackedObjects = Object.FindObjectsByType<PointOfInterest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            
            PointOfInterest hackedDevice = null;
            foreach (PointOfInterest poi in hackedObjects)
            {
                if(poi.AssignedDevice is null)
                    continue;
                
                if (poi.AssignedDevice.isHacked)
                    hackedDevice = poi;
            }
            
            return hackedDevice;
        }
        
        public override void Execute()
        {
            if (_destinationReached)     //Resolve the reached device
            {
                _solved = _hackedDevice.AssignedDevice.ResolveHack();

                if (_solved)
                {
                    _hackedDevice = FindHackedDevice();
                    if (_hackedDevice is not null)
                    {
                        _path = PathFinding.FindPath(AIUtilities.FindNearestWaypoint(Npc.transform.position), _hackedDevice);
                        _destinationReached = false;
                    }
                    else Npc.PreviousState();
                }
            }
            else                  //Reach the hacked device
            {
                
                Vector3 dir = _path[0].transform.position - Npc.transform.position;
                AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed);
                
                Vector3 forwardDir = Npc.transform.forward.normalized;
                Vector3 lookDir = dir;
                
                if (dir.magnitude < Npc.NpcDatas.DistanceThreshold && _path.Count > 1)
                   lookDir = _path[1].transform.position - Npc.transform.position;
                    
                AIUtilities.RotateTarget(Npc.transform, forwardDir, lookDir, Npc.NpcDatas.AngularSpeed);
            
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