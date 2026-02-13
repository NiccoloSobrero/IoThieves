using System.Collections.Generic;
using AI.AI_States.StateFamilies;
using AI.NavigationAndWaypoints;
using Unity.Cinemachine;
using UnityEngine;

namespace AI.AI_States
{
    public class NoiseHeardState : AIState, ITemporaryState, IInterruptableState, ISuspendable
    {
        private Transform _throwedObject;
        private List<Waypoint> _path;
        private bool _destinationReached = false;

        private float _turningThreshold = 1.0f;
        private float _nextWaypointThreshold = 0.5f;
        private float _throwedObjectSurroundings = 0.3f;

        private bool DestinationReached 
        {
            get => _destinationReached;
            set
            {
                if(_destinationReached == value) return;
                
                _destinationReached = value;
                RaiseOnWalkingVarChanged(!_destinationReached);
            }
        }
        public NoiseHeardState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            _throwedObject = null; //Some way to retrieve this info
            Waypoint nearestObj = AIUtilities.FindNearestWaypoint(_throwedObject.position);
            Waypoint nearestWaypoint = AIUtilities.FindNearestWaypoint(Npc.transform.position);
            
            Debug.Log("NoiseHeardState Waypoint: " + nearestWaypoint);
            _path = PathFinding.FindPath(nearestWaypoint, nearestObj);
            DestinationReached = false;
        }

        public override void Execute()
        {
            if (DestinationReached)     
            {
                //lookAround animation
                Debug.Log("Destination Reached");
                Npc.PreviousState();
            }
            else                  //Reach the throwed object 
            {
                Transform destination = (_path.Count > 0) ? _path[0].transform : _throwedObject;
                
                Vector3 dir = destination.position - Npc.transform.position;
                AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed); 
                
                Vector3 forwardDir = Npc.transform.forward.normalized;
                Vector3 lookDir = dir;
                
                if (dir.magnitude < _turningThreshold && _path.Count > 1)
                   lookDir = _path[1].transform.position - Npc.transform.position;

                lookDir.Normalize();
                AIUtilities.RotateTarget(Npc.transform, forwardDir, lookDir, Npc.NpcDatas.AngularSpeed);
            
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
                        DestinationReached = true;
                    }
                }
            }

        }

        public override void Exit()
        {
           
        }
        
    }
    
}
