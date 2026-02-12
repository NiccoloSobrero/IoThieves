using System.Collections.Generic;
using AI.AI_States.StateFamilies;
using AI.NavigationAndWaypoints;
using Unity.VisualScripting;
using UnityEngine;

namespace AI.AI_States
{
    public class GoToDestination : AIState, IExclusiveState 
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
        
        public GoToDestination(AIController npc) : base(npc) { }

        public override void Enter()
        {
            
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

        private void NextWaypoint()
        {
            if (_waypoints is null || _waypoints.Count == 0) 
            {
                Npc.ChangeState(AINames.Idle);
                return;
            } //Destination reached or no path available;
            
            //else go to next
            
            _currentWaypoint = _waypoints[0];
            _waypoints.RemoveAt(0);
        }

        public void SetDestination(Waypoint waypoint)
        {
            _Destination = waypoint;
            Waypoint nearest = AIUtilities.FindNearestWaypoint(Npc.transform.position);
            Debug.Log("Go to Destination");
            _waypoints = PathFinding.FindPath(nearest, _Destination);
            
            NextWaypoint();
        }

        public override void Exit()
        {
            
        }
    }
}