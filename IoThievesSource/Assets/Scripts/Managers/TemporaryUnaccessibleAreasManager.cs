using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(BoxCollider))]
    public class TemporaryUnaccessibleAreasManager : MonoBehaviour
    {
        public TemporaryUnaccessibleAreasManager instance;
        private static Collider[] _collider;
        private static bool _collidersMode = true;

        private void Awake()
        {
            if (instance is null)
            {
                instance = this;
                _collider = GetComponents<Collider>(); 
                // Debug.Log("I found "+ _collider.Length + " areas to protect ");
            }
            else
                Destroy(gameObject);
        }


        public static bool IsAccessible(Waypoint wp)
        {
            if (!_collidersMode)
                return true;
            
            foreach (Collider bc in _collider)
                if (bc.bounds.Contains(wp.transform.position))
                    return false;
            
            return true;
        }

        //AL TERMINE DEI DIALOGHI BISOGNA Disabilitare questo coso e riaccenderlo quando i dialoghi ripartono
        public static void Turn(bool mode)
        {
           _collidersMode = mode; 
        }
    }
}
