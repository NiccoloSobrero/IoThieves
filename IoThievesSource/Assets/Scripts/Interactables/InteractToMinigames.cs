using System.Collections.Generic;
using Levels;
using Managers;
using Minigames.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToMinigames : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<MinigameAndObjective> minigames = new();
        [SerializeField] private bool redoCompletedMinigameAfterReopen;

        #region Events
        
        [HideInInspector] public UnityEvent onInteract;

        #endregion
        
        public void Interact()
        {
            MinigamesManager.Instance.SetMinigamesLoop(this, minigames);

            onInteract?.Invoke();
        }
        
        public void TryRemovingMinigame(MinigameAndObjective minigameAndObjective)
        {
            if (redoCompletedMinigameAfterReopen) return;
            
            minigames.Remove(minigameAndObjective);
        }
    }

    [System.Serializable]
    public class MinigameAndObjective
    {
        public MinigameScrObj minigameScrObj;
        public LevelObjectiveScrObj objectiveScrObj; // Can be null
    }
}