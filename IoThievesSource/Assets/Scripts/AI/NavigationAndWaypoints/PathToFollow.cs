using System.Collections.Generic;
using UnityEngine;

namespace AI.NavigationAndWaypoints
{
    public class PathToFollow : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> waypoints = new ();

        public List<Waypoint> GetPath()
        {
            return waypoints;
        }
    }
}

