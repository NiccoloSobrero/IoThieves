using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using AI.NavigationAndWaypoints;
using Interactables;
using UnityEngine;

namespace AI.AI_States
{
    public class DeviceHackedState : AIState
    {
        
        private List<PointOfInterest> _hackedDevices = new  List<PointOfInterest>();
        private List<Waypoint> _path;
        private bool _destinationReached = true;

        private bool DestinationReached 
        {
            get => _destinationReached;
            set
            {
                if(_destinationReached == value) return;
                
                _destinationReached = value;
                RaiseOnWalkingVarChanged(!_destinationReached); //if destination is reached --> no need to walk else -->  you need to walk
            }
        }

        private float _nextWaypointThreshold = 0.5f;

        private bool _solved = false;
        
        public DeviceHackedState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            if (_hackedDevices.Count > 0) 
            {
                _path = PathFinding.FindPath(AIUtilities.FindNearestWaypoint(Npc.transform.position), _hackedDevices[0]);
                DestinationReached = false;
            }
        }
        
        public override void Execute()
        {
            if (DestinationReached)     //Resolve the reached device
            {
                _solved = _hackedDevices[0].AssignedDevice.ResolveHack();
                if (_solved)
                {
                    _hackedDevices.RemoveAt(0);
                    if (_hackedDevices.Count > 0)
                    {
                        _path = PathFinding.FindPath(AIUtilities.FindNearestWaypoint(Npc.transform.position), _hackedDevices[0]);
                        DestinationReached = false;
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

                    if (reached.Equals(_hackedDevices[0]))
                    {
                        DestinationReached = true;
                    }
                }
            }
        }
        
        public override void Exit()
        {
        }

        public void SetDevice(PointOfInterest pointOfInterest)
        {
            _hackedDevices.Add(pointOfInterest);
        }
    }
}