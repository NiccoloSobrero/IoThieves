using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public static class EventsManager
    {
        #region Gameplay Events
        
        public static readonly UnityEvent<Vector3> OnPointerClickInWorld = new();
        public static readonly UnityEvent OnMinigameCompleted = new();
        public static readonly UnityEvent OnGameOverTriggered = new();
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
        public static readonly UnityEvent<Level> OnPlayLevel = new();
        public static readonly UnityEvent OnBackToMainMenu = new();
        public static readonly UnityEvent OnEnterLevelSelection = new();
        public static readonly UnityEvent OnSaveGame = new();
        public static readonly UnityEvent OnPauseGame = new();
        public static readonly UnityEvent OnResumeGame = new();
        public static readonly UnityEvent OnQuitGame = new();
        public static readonly UnityEvent OnOptionsClicked = new();
        public static readonly UnityEvent OnRestartGame = new();
        public static readonly UnityEvent OnEndLevel = new();
        public static readonly UnityEvent OnOptionsSave = new();
        public static readonly UnityEvent OnNextLevel = new();
        public static readonly UnityEvent OnGameCompleted = new();
        # endregion


        #region Tutorial Events

        public static readonly UnityEvent OnFoundCodePadSolution = new();

        #endregion

        #region Dialogue Events

        public static readonly UnityEvent OnTriggerOnLineStart = new();
        public static readonly UnityEvent OnTriggerOnLineEnd = new();
        public static readonly UnityEvent OnTriggerOnContinueDialogue = new();

        #endregion

        #region Player Thrower Events

        public static readonly UnityEvent OnThrow = new();
        public static readonly UnityEvent OnIncreaseThrowDistance = new();
        public static readonly UnityEvent OnDecreaseThrowDistance = new();

        #endregion
    }
}
