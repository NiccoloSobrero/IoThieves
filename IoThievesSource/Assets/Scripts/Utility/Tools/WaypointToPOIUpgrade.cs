#if UNITY_EDITOR
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

        // STEP 1 — Rewire ALL references FIRST
        RewireAllNeighbors(oldWp, poi);

        // STEP 2 — Copy neighbour list
        CopyWaypointData(oldWp, poi);

        // STEP 3 — Destroy old component
        Undo.DestroyObjectImmediate(oldWp);

        Debug.Log($"Waypoint upgraded → PointOfInterest on {selected.name}");
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

        // Cache neighbour list BEFORE destruction
        var cachedNeighbours = new System.Collections.Generic.List<Waypoint>();
        if (oldPoi.neighbours != null)
            cachedNeighbours.AddRange(oldPoi.neighbours);

        // TEMP reference for rewiring
        var tempOld = oldPoi;

        // Destroy POI first (critical due to inheritance)
        Undo.DestroyObjectImmediate(oldPoi);

        // Add fresh Waypoint
        var wp = Undo.AddComponent<Waypoint>(selected);

        // Restore neighbour list
        wp.neighbours = new System.Collections.Generic.List<Waypoint>(cachedNeighbours);

        // Rewire all scene references
        RewireAllNeighbors(tempOld, wp);

        EditorUtility.SetDirty(wp);

        Debug.Log($"PointOfInterest downgraded → Waypoint on {selected.name}");
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

    static void CopyWaypointData(Waypoint src, Waypoint dst)
    {
        if (src.neighbours == null) return;

        if (dst.neighbours == null)
            dst.neighbours = new System.Collections.Generic.List<Waypoint>();
        else
            dst.neighbours.Clear();

        foreach (var wp in src.neighbours)
        {
            dst.neighbours.Add(wp);
        }

        EditorUtility.SetDirty(dst);
    }
}
#endif
