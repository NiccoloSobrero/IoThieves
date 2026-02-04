using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class BackToPreviousPosition : AIState
    {
        private Vector3 _finalDestination;
        private Waypoint _Destination;
        private List<Waypoint> _waypoints;
        private Vector3 _currentDestination;
        private bool _hasDestination = false;
        
        private Quaternion _lastRotation;
        private bool _hasRotation = false;


        private bool _isMoving = false;
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
        
        public BackToPreviousPosition(AIController npc) : base(npc) { }

        public override void Enter()
        {
            _finalDestination = Npc.lastPosition;
            Waypoint waypoint = AIUtilities.FindNearestWaypoint(_finalDestination);

            _hasRotation = Npc.matchRotationWhenComingBack;
            if (_hasRotation)
                _lastRotation = Npc.lastRotation;
                
            SetDestination(waypoint);
        }
        
        
        public override void Execute()
        {
            if (!_hasDestination)
            {
                if (!_hasRotation)
                    Npc.PreviousState();

                float maxStep = Npc.NpcDatas.AngularSpeed * Time.deltaTime *45.0f;

                Npc.transform.rotation = Quaternion.RotateTowards(
                    Npc.transform.rotation,
                    _lastRotation,
                    maxStep
                );

                if (Quaternion.Angle(Npc.transform.rotation, _lastRotation) < 0.1f)
                {
                    Npc.transform.rotation = _lastRotation;
                    _hasRotation = false;
                }
  
                return;
            }
                
            
           Vector3 dir = _currentDestination - Npc.transform.position;
           dir.Normalize();

           Vector3 forwardDir =  Npc.transform.forward.normalized;
            
           float angleMag = AIUtilities.RotateTarget(Npc.transform, forwardDir, dir, Npc.NpcDatas.AngularSpeed);
            
           IsMoving = angleMag < Npc.NpcDatas.AngularThreshold;
            
           if(IsMoving)
               AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed);

           float distance =  Vector3.Distance(Npc.transform.position, _currentDestination);
            
           if (distance < Npc.NpcDatas.DistanceThreshold)
           {
               NextWaypoint(); 
           }
        }

        public void SetDestination(Waypoint waypoint)
        {
            _Destination = waypoint;
            Waypoint nearest = AIUtilities.FindNearestWaypoint(Npc.transform.position);
            Debug.Log("BackToPreviousPosition " + nearest);
            _waypoints = PathFinding.FindPath(nearest, _Destination);
            
            FirstWaypoint();
        }
        
        private void FirstWaypoint()
        {
            if (_waypoints is null || _waypoints.Count == 0)
            {
                Npc.PreviousState();
                return;
            }

            if (_waypoints.Count > 1 && AIUtilities.IsVisible(Npc.gameObject, _waypoints[1].gameObject))
            {
                Debug.Log("I Start from the second");
                _waypoints.RemoveAt(0);
            }
            
            _currentDestination = _waypoints[0].transform.position;
            _hasDestination = true;
            _waypoints.RemoveAt(0);
        }
        
        private void NextWaypoint()
        {
            if (_currentDestination == _finalDestination)
            {
                IsMoving = false;
                _hasDestination = false;
                if (!_hasRotation)
                {
                    Npc.PreviousState();
                }

                return;
            }
            
            if (_waypoints.Count == 0)
            {
                _currentDestination = _finalDestination; 
                return;
            } 
            
            _currentDestination = _waypoints[0].transform.position;
            _hasDestination = true;
            _waypoints.RemoveAt(0);
        }
  

        public override void Exit()
        {
            IsMoving = false;
            _waypoints = null;
            _Destination = null;
            //_currentDestination = null;
            _hasDestination = false;
        }
    }
}