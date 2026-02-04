using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class BackToPreviousPosition : AIState
    {
        private Waypoint _Destination;
        private List<Waypoint> _waypoints;
        private Waypoint _currentWaypoint;


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
            Waypoint waypoint = AIUtilities.FindNearestWaypoint(Npc.lastPosition); 
            SetDestination(waypoint);
        }
        
        
        public override void Execute()
        {
           if (_currentWaypoint is null)
                return;
            
           Vector3 dir = _currentWaypoint.transform.position - Npc.transform.position;
           dir.Normalize();

           Vector3 forwardDir =  Npc.transform.forward.normalized;
            
           float angleMag = AIUtilities.RotateTarget(Npc.transform, forwardDir, dir, Npc.NpcDatas.AngularSpeed);
            
           IsMoving = angleMag < Npc.NpcDatas.AngularThreshold;
            
           if(IsMoving)
               AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed);

           float distance =  Vector3.Distance(Npc.transform.position, _currentWaypoint.transform.position);
            
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
            
            _currentWaypoint = _waypoints[0];
            _waypoints.RemoveAt(0);
        }
        
        private void NextWaypoint()
        {
            if (_waypoints is null || _waypoints.Count == 0)
            {
                Npc.PreviousState();
                return;
            } 
            
            _currentWaypoint = _waypoints[0];
            _waypoints.RemoveAt(0);
        }
  

        public override void Exit()
        {
            IsMoving = false;
            _waypoints = null;
            _Destination = null;
            _currentWaypoint = null;
        }
    }
}