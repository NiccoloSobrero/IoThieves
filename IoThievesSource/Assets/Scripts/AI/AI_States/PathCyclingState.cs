using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class PathCyclingState : AIState
    {
        
        private float _speed = 1.0f;
        private float _angularSpeed = 1.0f;
        private float _distanceThreshold = 0.1f;
        private float _angularThreshold = 30.0f;
        
        private List<Waypoint> _path;
        private int _idx = 0;
        private Vector3 _targetPosition;
        private bool _isPath = false;
        private bool _isMoving = false;
        
        public PathCyclingState(AIController npc) : base(npc) {}
        

        public void SetPath(List<Waypoint> path)
        {
            if (path is null || path.Count == 0) return;
            _path = path;
            _isMoving = true;
            _idx = 0;
            _targetPosition = path[_idx].transform.position;
        }

        public void NextWaypoint()
        {
            _idx = (_idx + 1)%_path.Count;
            _targetPosition = _path[_idx].transform.position;
        }
        
        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Execute()
        {
            if (!_isMoving)
                return;
            
            Vector3 dir = _targetPosition - Npc.transform.position;
            dir.Normalize();
            
            Vector3 forwardDir =  Npc.transform.forward.normalized;
            
            float angleMag = AIUtilities.RotateTarget(Npc.transform, dir, forwardDir, _angularSpeed);
            
            if(angleMag < _angularThreshold)
                AIUtilities.MoveTarget(Npc.transform, dir, _speed);

            float distance =  Vector3.Distance(Npc.transform.position, _targetPosition);
            
            if (distance < _distanceThreshold)
            {
               NextWaypoint(); 
            }
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}