using UnityEngine;

namespace AI.NavigationAndWaypoints
{
    public class DrawWaypointGizmos : MonoBehaviour
    {
        [SerializeField] public bool render;
        
        [SerializeField] private Color gizmoColor;
        
        private void OnDrawGizmos()
        {
            if (!render) return;
            
            Gizmos.color = gizmoColor;

            Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();

            foreach (var waypoint in waypoints)
            {
                foreach (var neighbour in waypoint.neighbours)
                {
                    Gizmos.DrawLine(waypoint.transform.position, neighbour.transform.position);
                }
            }
        }
    }
}
