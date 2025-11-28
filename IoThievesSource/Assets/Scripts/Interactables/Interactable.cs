using System.Collections.Generic;
using Hackables;
using Managers;
using Minigames.ScriptableObjects;
using UnityEngine;

namespace Interactables
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private List<MinigameScrObj> minigameObjs = new();
        [SerializeField] private bool redoCompletedMinigameAfterReopen;
        
        public void Interact()
        {
            MinigamesManager.Instance.SetMinigamesLoop(this, minigameObjs);
            
            
        }
        
        public void TryRemovingMinigame(MinigameScrObj minigameScrObj)
        {
            if (redoCompletedMinigameAfterReopen) return;
            
            minigameObjs.Remove(minigameScrObj);
        }
        
    }
}