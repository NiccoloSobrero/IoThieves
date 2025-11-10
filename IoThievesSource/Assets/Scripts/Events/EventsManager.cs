using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public static class EventsManager
    {
        #region Gameplay Events
        public static readonly UnityEvent<Vector3> OnPointerClickInWorld = new();
        public static readonly UnityEvent<GameObject> OnHackableInteraction = new();
        #endregion
        
        #region Save and Load Events
        public static readonly UnityEvent<string> OnGameSaved = new();
        public static readonly UnityEvent<string> OnGameLoaded = new();
        public static readonly UnityEvent<string> OnGameSaveError = new();
        public static readonly UnityEvent<string> OnGameLoadError = new();
        #endregion
    }
}
