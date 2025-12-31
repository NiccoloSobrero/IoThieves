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
        [SerializeField] private bool debugMode = true;
    
        [ContextMenu("Generate Connections")]
        public void GenerateConnections()
        {
            Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();
        
            // Clear existing
            foreach (Waypoint wp in allWaypoints)
            {
                wp.neighbours.Clear();
#if UNITY_EDITOR
                EditorUtility.SetDirty(wp);
#endif
            }
        
            int connections = 0;
        
            // Check each pair
            for (int i = 0; i < allWaypoints.Length; i++)
            {
                for (int j = i + 1; j < allWaypoints.Length; j++)
                {
                    if (HasLineOfSight(allWaypoints[i], allWaypoints[j]))
                    {
                        allWaypoints[i].neighbours.Add(allWaypoints[j]);
                        allWaypoints[j].neighbours.Add(allWaypoints[i]);
                        connections++;
#if UNITY_EDITOR
                        EditorUtility.SetDirty(allWaypoints[i]);
                        EditorUtility.SetDirty(allWaypoints[j]);
#endif
                    
                        if (debugMode)
                            Debug.Log($"✓ Connected: {allWaypoints[i].name} <-> {allWaypoints[j].name}");
                    }
                    else if (debugMode)
                    {
                        Debug.Log($"✗ Blocked: {allWaypoints[i].name} <-> {allWaypoints[j].name}");
                    }
                }
            }
        
            Debug.Log($"Generated {connections} connections for {allWaypoints.Length} waypoints");
        }
    
        private bool HasLineOfSight(Waypoint from, Waypoint to)
        {
            Vector3 startPos = from.transform.position;
            Vector3 endPos = to.transform.position;
            float distance = Vector3.Distance(startPos, endPos);
        
            if (distance > maxConnectionDistance)
            {
                if (debugMode)
                    Debug.Log($"  Reason: Too far ({distance:F2}m > {maxConnectionDistance}m)");
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
                    if (debugMode)
                        Debug.Log($"  Reason: Blocked by {col.gameObject.name} on layer {LayerMask.LayerToName(col.gameObject.layer)}");
                    return false;
                }
            }
        
            return true;
        }
    
        public void ClearAllConnections()
        {
            Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();
        
            foreach (Waypoint wp in allWaypoints)
            {
                wp.neighbours.Clear();
                EditorUtility.SetDirty(wp);
            }
        
            Debug.Log($"Cleared all connections for {allWaypoints.Length} waypoints.");
        }
    
        [ContextMenu("Rename All Waypoints")]
        public void RenameAllWaypoints()
        {
            Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();
        
            for (int i = 0; i < allWaypoints.Length; i++)
            {
                allWaypoints[i].gameObject.name = $"Waypoint ({i + 1})";
#if UNITY_EDITOR
                EditorUtility.SetDirty(allWaypoints[i].gameObject);
#endif
            }
        
            Debug.Log($"Renamed {allWaypoints.Length} waypoints");
        } 
    
        private void OnDrawGizmosSelected()
        {
            // Draw the cylinder radius for visualization
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, cylinderRadius);
        }
    }
}