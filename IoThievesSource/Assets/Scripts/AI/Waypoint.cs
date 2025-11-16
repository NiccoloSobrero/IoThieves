using System.Collections.Generic;

using UnityEngine;

namespace AI
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> neighbours; 
        
        public Waypoint GetNextPatrolTarget()
        {
            int randomIndex = Random.Range(0, neighbours.Count);
            return neighbours[randomIndex];
        }
    }
}