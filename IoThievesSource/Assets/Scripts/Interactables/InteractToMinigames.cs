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
        
        private TutorialObjective _onInteractObjective;
        
        public void Interact()
        {
            MinigamesManager.Instance.SetMinigamesLoop(this, minigames);

            if (_onInteractObjective != null)
            {
                _onInteractObjective.Complete();
                _onInteractObjective = null;
            }
        }
        
        public void TryRemovingMinigame(MinigameAndObjective minigameAndObjective)
        {
            if (redoCompletedMinigameAfterReopen) return;
            
            minigames.Remove(minigameAndObjective);
        }

        public void SetOnInteractObjective(TutorialObjective objective)
        {
            _onInteractObjective = objective;
        }
    }

    [System.Serializable]
    public class MinigameAndObjective
    {
        public MinigameScrObj minigameScrObj;
        public LevelObjectiveScrObj objectiveScrObj; // Can be null
    }
}