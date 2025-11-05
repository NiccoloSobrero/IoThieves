using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public static class EventsManager
    {
        public static readonly UnityEvent<Vector3> OnPointerClickInWorld = new();
        public static readonly UnityEvent<GameObject> OnHackableInteraction = new();
    }
}
