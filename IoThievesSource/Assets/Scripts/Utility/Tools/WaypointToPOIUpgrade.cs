using AI.NavigationAndWaypoints;
using UnityEditor;
using UnityEngine;

public static class UpgradeWaypointToPOI
{
    [MenuItem("Tools/Waypoint Graph/Upgrade Selected To PointOfInterest")]
    public static void Upgrade()
    {
        var selected = Selection.activeGameObject;

        if (!selected)
        {
            Debug.LogError("Select a GameObject containing a Waypoint.");
            return;
        }

        var oldWp = selected.GetComponent<Waypoint>();
        if (!oldWp)
        {
            Debug.LogError("Selected object does not contain Waypoint.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selected, "Upgrade Waypoint To POI");

        // Add POI
        var poi = selected.GetComponent<PointOfInterest>();
        if (!poi)
            poi = Undo.AddComponent<PointOfInterest>(selected);

        // Copy data from old waypoint if needed
        CopyWaypointData(oldWp, poi);

        // Rewire all neighbors across scene
        RewireAllNeighbors(oldWp, poi);

        // Remove old waypoint
        Undo.DestroyObjectImmediate(oldWp);

        Debug.Log($"Waypoint upgraded → PointOfInterest on {selected.name}");
    }

    static void RewireAllNeighbors(Waypoint oldWp, Waypoint newWp)
    {
        var allWaypoints = GameObject.FindObjectsOfType<Waypoint>(true);

        int rewired = 0;

        foreach (var wp in allWaypoints)
        {
            if (wp.neighbours == null) continue;

            Undo.RecordObject(wp, "Rewire Waypoint Graph");

            for (int i = 0; i < wp.neighbours.Count; i++)
            {
                if (wp.neighbours[i] == oldWp)
                {
                    wp.neighbours[i] = newWp;
                    rewired++;
                }
            }

            EditorUtility.SetDirty(wp);
        }

        Debug.Log($"Rewired {rewired} neighbor references.");
    }

    /*
     * Copies the references from the two waypoints components
     */
    static void CopyWaypointData(Waypoint src, Waypoint dst)
    {
        foreach (var wp in src.neighbours)
        {
            dst.neighbours.Add(wp);
        }
    }
    
    [MenuItem("Tools/Waypoint Graph/Downgrade Selected To Waypoint")]
    public static void Downgrade()
    {
        var selected = Selection.activeGameObject;

        if (!selected)
        {
            Debug.LogError("Select a GameObject containing PointOfInterest.");
            return;
        }

        var oldPoi = selected.GetComponent<PointOfInterest>();
        if (!oldPoi)
        {
            Debug.LogError("Selected object does not contain PointOfInterest.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selected, "Downgrade POI To Waypoint");

        // Add Waypoint
        var wp = selected.GetComponent<Waypoint>();
        if (!wp)
            wp = Undo.AddComponent<Waypoint>(selected);

        // Copy data back
        CopyWaypointData(oldPoi, wp);

        // Rewire neighbors across scene
        RewireAllNeighbors(oldPoi, wp);

        // Remove POI
        Undo.DestroyObjectImmediate(oldPoi);

        Debug.Log($"PointOfInterest downgraded → Waypoint on {selected.name}");
    }
 
}


