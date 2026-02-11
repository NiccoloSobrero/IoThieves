using System;
using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class PathCyclingState : AIState
    {
        private List<Waypoint> _path;
        private int _idx = 0;
        private Vector3 _targetPosition;
        private bool _hasPath = false;
        private bool _isMoving = false;
        private bool _backToPath = false;
        private List<Waypoint> _backPath;

        private bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value) return;
                
                _isMoving = value;
                RaiseOnWalkingVarChanged(_isMoving);
            } 
        }

        public PathCyclingState(AIController npc) : base(npc)
        {
            SetPath();
        }


        private void SetPath()
        {
            try
            {
                PathToFollow ptf = Npc.GetComponent<PathToFollow>();
                if (ptf is null)
                {
                    return;
                }
                
                List<Waypoint> path = ptf.GetPath();
                if (path is null || path.Count == 0) return;
                
                _path = path;
                _hasPath = true; 
                _idx = 0;
                _targetPosition = _path[_idx].transform.position;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void NextWaypoint()
        {
            if (_backToPath)
            {
                try
                {
                    if (_backPath == null || _backPath.Count == 0)
                    {
                        _backToPath = false;
                    }
                    else
                    {
                        _targetPosition = _backPath[0].transform.position; 
                        _backPath.RemoveAt(0);
                    }
                }
                catch (ArgumentOutOfRangeException) 
                {
                    _backToPath = false;
                }
            }
            else
            {
                _idx = (_idx + 1)%_path.Count;
                _targetPosition = _path[_idx].transform.position;
            }
        }
        
        public override void Enter()
        {
            float distanceFromTarget = Vector3.Distance(Npc.transform.position, _targetPosition);
            if (distanceFromTarget > 2.0f)   //If I am far from the path -> PathFind back to the path to follow
            {
                _backToPath = true;
                Waypoint target =  _path[_idx];
                if(!Physics.Raycast(Npc.transform.position, _targetPosition - Npc.transform.position, 
                       distanceFromTarget, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
                {
                    _backPath = new List<Waypoint>(){target};
                }
                else
                {
                    Waypoint start = AIUtilities.FindNearestWaypoint(Npc.transform.position);
                    _backPath = PathFinding.FindPath(start, _path[_idx]);
                }
                
                NextWaypoint();
            }
            
            IsMoving = true;
        }

        public override void Execute()
        {
            if (!_hasPath || !AIUtilities.IsValidPath(_path))
            {
                return;
            }
            
            Vector3 dir = _targetPosition - Npc.transform.position;
            dir.Normalize();

            Vector3 forwardDir =  Npc.transform.forward.normalized;
            
            float angleMag = AIUtilities.RotateTarget(Npc.transform, forwardDir, dir, Npc.NpcDatas.AngularSpeed);
            
            IsMoving = angleMag < Npc.NpcDatas.AngularThreshold;
            if(IsMoving)
                AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed);

            float distance =  Vector3.Distance(Npc.transform.position, _targetPosition);
            
            if (distance < Npc.NpcDatas.DistanceThreshold)
            {
               NextWaypoint(); 
            }
        }

        public override void Exit()
        {
            IsMoving = false;
        }
    }
}