using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public static class EventsManager
    {
        #region Gameplay Events
        
        public static readonly UnityEvent<Vector3> OnPointerClickInWorld = new();
        public static readonly UnityEvent OnMinigameCompleted = new();
        
        #endregion
        
        
        #region Save and Load Events
        
        public static readonly UnityEvent<string> OnGameSaved = new();
        public static readonly UnityEvent<string> OnGameLoaded = new();
        public static readonly UnityEvent<string> OnGameSaveError = new();
        public static readonly UnityEvent<string> OnGameLoadError = new();
        
        #endregion
        
        
        #region Minigames

        public static readonly UnityEvent<string> OnClickDecryptPassword = new();

        #endregion
        
        
        # region UI Events
        public static readonly UnityEvent OnStartGame = new();
        public static readonly UnityEvent OnSaveGame = new();
        public static readonly UnityEvent OnPauseGame = new();
        public static readonly UnityEvent OnResumeGame = new();
        public static readonly UnityEvent OnQuitGame = new();
        public static readonly UnityEvent OnOptionsClicked = new();
        public static readonly UnityEvent OnRestartGame = new();
        public static readonly UnityEvent OnEndLevel = new();
        # endregion


        #region Tutorial Events

        public static readonly UnityEvent OnFoundCodePadSolution = new();

        #endregion
    }
}
