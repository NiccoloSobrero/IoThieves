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
        private bool _isPath = false;
        private bool _isMoving = false;
        private bool back_to_path = false;
        private List<Waypoint> _backPath;

        public PathCyclingState(AIController npc) : base(npc)
        {
            SetPath();
        }
        

        public void SetPath()
        {
            try
            {
                List<Waypoint> path = Npc.GetComponent<PathToFollow>().GetPath();
                if (path is null || path.Count == 0) return;
                _path = path;
                _isMoving = true;
                _idx = 0;
                _targetPosition = _path[_idx].transform.position;
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
                catch (ArgumentOutOfRangeException e)  //BackPath fully exhausted
                {
                    back_to_path = false;
                }
            }
            else
            {
                _idx = (_idx + 1)%_path.Count;
                _targetPosition = _path[_idx].transform.position;
            }
            Debug.Log(_targetPosition);
        }
        
        public override void Enter()
        {
            if (Vector3.Distance(Npc.transform.position, _targetPosition) > 2.0f)   //If I am far from the path -> PathFind back to the path to follow
            {
                Debug.Log("BACKPATH??");
                back_to_path = true;
                _backPath = PathFinding.FindPath(AIUtilities.FindNearestWaypoint(Npc.transform.position), _path[_idx]);
                
                NextWaypoint();
            }
            //Else I am already near enough to my path
        }

        public override void Execute()
        {
            if (!_isMoving)
                return;
            
            Vector3 dir = _targetPosition - Npc.transform.position;
            dir.Normalize();
            
            Vector3 forwardDir =  Npc.transform.forward.normalized;
            
            float angleMag = AIUtilities.RotateTarget(Npc.transform, forwardDir, dir, Npc.NpcDatas.AngularSpeed);
            
            if(angleMag < Npc.NpcDatas.AngularThreshold)
                AIUtilities.MoveTarget(Npc.transform, dir, Npc.NpcDatas.Speed);

            
            float distance =  Vector3.Distance(Npc.transform.position, _targetPosition);
            
            if (distance < Npc.NpcDatas.DistanceThreshold)
            {
               NextWaypoint(); 
            }
        }

        public override void Exit()
        {
           //Nothing to do there 
        }
    }
}