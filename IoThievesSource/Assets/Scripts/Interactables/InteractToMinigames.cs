using System;
using System.Collections.Generic;
using Levels;
using Managers;
using Minigames.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToMinigames : Interactable
    {
        [SerializeField] private List<MinigameAndObjective> minigames = new();
        [SerializeField] private bool redoCompletedMinigameAfterReopen;

        [SerializeField] private LayerMask playerLayer;
        
        [SerializeField] private InteractableTriggerArea triggerArea;

        #region Events
        
        [HideInInspector] public UnityEvent onInteract;

        #endregion

        private void Awake()
        {
            triggerArea.Init(this);
        }

        public override void Interact()
        {
            if (!triggerArea.IsPlayerInRange) return;
            
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