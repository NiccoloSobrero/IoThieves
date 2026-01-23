using System;
using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WalkableArea : MonoBehaviour
{
    private Collider _area;
    private Dictionary<Waypoint,bool> _waypoints = new Dictionary<Waypoint, bool>();

    private void Awake()
    {
       _area = GetComponent<Collider>(); 
       Debug.Log(_area);
       Waypoint[] waypoints = FindObjectsByType<Waypoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
       foreach (var wp in waypoints)
       {
           _waypoints.Add(wp,_area.bounds.Contains(wp.transform.position));
       }
    }

    public Waypoint GetRandomValidwaypoint()
    {
        foreach (Waypoint wp in _waypoints.Keys)
        {
            if (_waypoints[wp])
                return wp;
        }

        return null;
    }
    public bool IsInside(Transform t)
    {
        return _area.bounds.Contains(t.position);
    }
    public bool IsWalkable(Waypoint wp)
    {
        return _waypoints[wp];
    }
}
