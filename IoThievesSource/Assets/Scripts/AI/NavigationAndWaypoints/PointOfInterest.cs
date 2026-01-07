using Interactables;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace AI.NavigationAndWaypoints
{
    public class PointOfInterest : Waypoint
    {
       
        [SerializeField] private Device assignedDevice;

        public Device AssignedDevice => assignedDevice;
    }
}