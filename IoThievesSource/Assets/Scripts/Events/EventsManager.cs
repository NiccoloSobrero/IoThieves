using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public static class EventsManager
    {
        public static readonly UnityEvent<Vector3> OnPointerClick =  new();
    }
}
