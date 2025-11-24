using System.Collections.Generic;
using Managers;
using Minigames.ScriptableObjects;
using UnityEngine;

namespace Hackables
{
    public class HackableWithMinigames : MonoBehaviour, IHackable
    {
        public List<MinigameScrObj> minigameObjs = new();
        
        public void Hack()
        {
            MinigamesManager.Instance.MinigamesQueue.Clear();
            
            MinigamesManager.Instance.EnableWindow(true);
            
            foreach (MinigameScrObj minigameScrObj in minigameObjs)
            {
                MinigamesManager.Instance.MinigamesQueue.Enqueue(minigameScrObj);
            }

            MinigamesManager.Instance.MoveToNextMinigame();
        }
    }
}