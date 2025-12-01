using Interactables;
using Levels;
using UnityEngine;

namespace Minigames.ScriptableObjects
{
    public abstract class MinigameScrObj : ScriptableObject
    {
        public abstract void InitMinigame(LevelObjectiveScrObj objectiveScrObj);
    }
}