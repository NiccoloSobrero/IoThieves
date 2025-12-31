using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace AI.Utilities
{
    [CustomEditor(typeof(WaypointConnector))]
    public class WaypointConnectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            WaypointConnector connector = (WaypointConnector)target;
        
            EditorGUILayout.Space();
        
            if (GUILayout.Button("Generate Waypoint Connections", GUILayout.Height(40)))
            {
                connector.GenerateConnections();
            }
        
            if (GUILayout.Button("Clear All Connections", GUILayout.Height(30)))
            {
                connector.ClearAllConnections();
            }
            if (GUILayout.Button("RenameAll", GUILayout.Height(20)))
            {
                connector.RenameAllWaypoints();
            }
        }
    }
}
#endif
