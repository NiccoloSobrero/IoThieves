using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace AI.NavigationAndWaypoints
{
    public class PointOfInterest : Waypoint
    {
        [SerializeField] private int interactTime;
        [SerializeField] private float thisValue = 0;
        [SerializeField] private GameObject assignedDevice;

        public GameObject AssignedDevice => assignedDevice;

        public bool interact()
        {
            thisValue += (1.0f/interactTime)*Time.deltaTime;
            if (thisValue >= 1)
            {
                thisValue = 0;
                return true;
            }

            return false;
        }
    }
}