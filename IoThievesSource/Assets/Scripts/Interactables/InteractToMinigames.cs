using System.Collections.Generic;
using Levels;
using Managers;
using Minigames.ScriptableObjects;
using UnityEngine;

namespace Interactables
{
    public class InteractToMinigames : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<MinigameAndObjective> minigames = new();
        [SerializeField] private bool redoCompletedMinigameAfterReopen;
        
        public void Interact()
        {
            MinigamesManager.Instance.SetMinigamesLoop(this, minigames);
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