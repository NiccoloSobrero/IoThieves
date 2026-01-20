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
        private bool _destinationReached = false;
        private float THRESHOLD = 5;
        private bool lookingAt = false;
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
                Debug.Log("Ho un device ed è "+ _hackedDevices[0]);
                Waypoint nearestToMe = AIUtilities.FindNearestWaypoint(Npc.transform.position);
                Debug.Log("Waypoint più vicino a me is :" + nearestToMe);
                _path = PathFinding.FindPath(nearestToMe, _hackedDevices[0]);
                if(_path is not null && _path.Count > 0)
                    Debug.Log("Ho trovato un percorso valido");
                else
                {
                    Debug.Log("NON HO TROVATO UN PERCORSO SONO UN PAZZO");
                }
                DestinationReached = false;
                RaiseOnWalkingVarChanged(!DestinationReached);
            }
        }
        
        public override void Execute()
        {
            if (DestinationReached)     //Resolve the reached device
            {
                if (!lookingAt)
                {
                    Vector3 npcT = new Vector3(Npc.transform.position.x, 0, Npc.transform.position.z);
                    Vector3 d = _hackedDevices[0].AssignedDevice.transform.position;
                    Vector3 deviceDir = new Vector3(d.x-npcT.x,0,d.z-npcT.z);
                
                    float angleMagnitude = AIUtilities.RotateTarget(Npc.transform, Npc.transform.forward, deviceDir, Npc.NpcDatas.AngularSpeed);
                    lookingAt = IsLookingAt(d); 
                    return;
                }

                
                _solved = _hackedDevices[0].AssignedDevice.ResolveHack();
                if (_solved && lookingAt)
                {
                    lookingAt = false;
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
                        lookingAt = IsLookingAt(reached.transform.position);
                    }
                }
            }
        }

        private bool IsLookingAt(Vector3 targetPos)
        {
            Vector3 tmp = new Vector3(targetPos.x, 0, targetPos.z);
            Debug.Log("Device position is " +targetPos + "\nPalle : "+ Npc.transform.position);
            float angle = Vector3.Angle(Npc.transform.forward, tmp - Npc.transform.position);
            Debug.Log("angle : " + angle);
            bool prova = angle  < THRESHOLD;
                Debug.Log("Sto guardando il device? "+ prova);
            return prova;
        }


        public override void Exit()
        {
        }

        public void SetDevice(PointOfInterest pointOfInterest)
        {
            Debug.Log("Device added");
            _hackedDevices.Add(pointOfInterest);
        }
    }
}