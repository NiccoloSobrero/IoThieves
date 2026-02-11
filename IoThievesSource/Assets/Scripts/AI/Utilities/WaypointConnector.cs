using AI.NavigationAndWaypoints;
using UnityEditor;
using UnityEngine;

namespace AI.Utilities
{
    public class WaypointConnector : MonoBehaviour
    {
        [SerializeField] private float cylinderRadius = 0.2f;
        [SerializeField] private LayerMask obstacleLayers = ~0;
        [SerializeField] private float maxConnectionDistance = 50f;
    
        [ContextMenu("Generate Connections")]
        public void GenerateConnections()
        {
            Waypoint[] allWaypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        
            // Clear existing
            foreach (Waypoint wp in allWaypoints)
            {
                wp.neighbours.Clear();
#if UNITY_EDITOR
                EditorUtility.SetDirty(wp);
#endif
            }
        
            // Check each pair
            for (int i = 0; i < allWaypoints.Length; i++)
            {
                for (int j = i + 1; j < allWaypoints.Length; j++)
                {
                    if (HasLineOfSight(allWaypoints[i], allWaypoints[j]))
                    {
                        if (!allWaypoints[i].neighbours.Contains(allWaypoints[j]))
                        {
                            allWaypoints[i].neighbours.Add(allWaypoints[j]);
                            allWaypoints[j].neighbours.Add(allWaypoints[i]);
                        }
                        
#if UNITY_EDITOR
                        EditorUtility.SetDirty(allWaypoints[i]);
                        EditorUtility.SetDirty(allWaypoints[j]);
#endif
                    }
                }
            }
        }
    
        private bool HasLineOfSight(Waypoint from, Waypoint to)
        {
            Vector3 startPos = from.transform.position;
            Vector3 endPos = to.transform.position;
            float distance = Vector3.Distance(startPos, endPos);
        
            if (distance > maxConnectionDistance)
            {
                return false;
            }
        
            // Check if the cylinder/tunnel between the two points is clear
            Collider[] hits = Physics.OverlapCapsule(
                startPos, 
                endPos, 
                cylinderRadius, 
                obstacleLayers
            );
        
            // Filter out the waypoint objects themselves
            foreach (Collider col in hits)
            {
                if (col.gameObject != from.gameObject && 
                    col.gameObject != to.gameObject)
                {
                    return false;
                }
            }
        
            return true;
        }
    
        public void ClearAllConnections()
        {
            Waypoint[] allWaypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        
            foreach (Waypoint wp in allWaypoints)
            {
                wp.neighbours.Clear();
                // EditorUtility.SetDirty(wp);
            }
        }
    
        [ContextMenu("Rename All Waypoints")]
        public void RenameAllWaypoints()
        {
            Waypoint[] allWaypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        
            for (int i = 0; i < allWaypoints.Length; i++)
            {
                allWaypoints[i].gameObject.name = $"Waypoint ({i + 1})";
#if UNITY_EDITOR
                EditorUtility.SetDirty(allWaypoints[i].gameObject);
#endif
            }
        } 
    
        private void OnDrawGizmosSelected()
        {
            // Draw the cylinder radius for visualization
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, cylinderRadius);
        }
    }
}