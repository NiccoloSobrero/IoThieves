using System;
using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using NUnit.Framework;
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
        private bool back_to_path = false;
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
        

        public void SetPath()
        {
            try
            {
                PathToFollow ptf = Npc.GetComponent<PathToFollow>();
                if (ptf is null)
                {
                    Debug.Log("There is no path to follow");
                    return;
                }
                
                List<Waypoint> path = ptf.GetPath();
                if (path is null || path.Count == 0) return;
                
                _path = path;
                _hasPath = true; 
                _idx = 0;
                _targetPosition = _path[_idx].transform.position;
                Debug.Log("Path is " + _path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void NextWaypoint()
        {
            if (back_to_path)
            {
                try
                {
                    _targetPosition = _backPath[0].transform.position; 
                    _backPath.RemoveAt(0);
                }
                catch (ArgumentOutOfRangeException _)  //BackPath fully exhausted
                {
                    back_to_path = false;
                }
            }
            else
            {
                _idx = (_idx + 1)%_path.Count;
                _targetPosition = _path[_idx].transform.position;
            }
            //Debug.Log(_targetPosition);
        }
        
        public override void Enter()
        {
            float distanceFromTarget = Vector3.Distance(Npc.transform.position, _targetPosition);
            if (distanceFromTarget > 2.0f)   //If I am far from the path -> PathFind back to the path to follow
            {
                back_to_path = true;
                Waypoint target =  _path[_idx];
                if(!Physics.Raycast(Npc.transform.position, _targetPosition - Npc.transform.position, 
                       distanceFromTarget, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
                {
                    _backPath = new List<Waypoint>(){target};
                }
                else
                {
                    Waypoint start = AIUtilities.FindNearestWaypoint(Npc.transform.position);
                    Debug.Log("PathCyclingState Enter");
                    _backPath = PathFinding.FindPath(start, _path[_idx]);
                }
                
                NextWaypoint();
            }
            
            IsMoving = true;
        }

        public override void Execute()
        {
            if (!_hasPath)
                return;
            
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
           //Nothing to do there 
        }
    }
}