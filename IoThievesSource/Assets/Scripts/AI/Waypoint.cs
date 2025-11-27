using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] public List<Waypoint> neighbours;
   //     private Dictionary<Range, Waypoint> _waypoints =  new Dictionary<Range, Waypoint>();
   //     [SerializeField] private float reductionPerc = 18.0f;
  /*      private void Awake()
        {
            reductionPerc = 18.0f;
            float rangeSize = 100.0f/neighbours.Count;
            
            for (int i = 0; i < neighbours.Count; i++)
            {
                float startValue = i*rangeSize;
                _waypoints.Add(new Range(startValue, i < neighbours.Count-1 ? startValue+rangeSize : 100), 
                                neighbours[i]); 
            } 
        }

        public Waypoint GetNextPatrolTarget()
        {
            float randomValue = Random.Range(0.0f, 100.0f);
            return ChooseTarget(randomValue);
        }

        private Waypoint ChooseTarget(float randomValue)
        {
            Waypoint target = null;
            foreach(Range r in _waypoints.Keys)
            {
                if (r.InRange(randomValue))
                {
                    target = _waypoints[r];
                    ResizeRanges(r);
                    break;
                }
            }
            return target;
        }

        private void ResizeRanges(Range range)
        {
            if (neighbours.Count == 1)
                return;

            float increment = reductionPerc/neighbours.Count-1; 
            float lastMax = 0;
            int counter = 0;
            foreach(Range r in _waypoints.Keys)
            {
                float min = r.Min;
                float max = r.Max;
                float interval = max - min;
                float newMin = lastMax;
                counter++;
                r.Edit(newMin,counter==neighbours.Count ? 100.0f : 
                                newMin+interval + (r.Equals(range) ? -reductionPerc : 
                                                    increment));
            }
        }*/
    }
}