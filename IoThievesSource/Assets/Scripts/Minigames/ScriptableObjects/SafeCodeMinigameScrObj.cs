using Managers;
using UnityEngine;

namespace Minigames.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SafeCodeMinigame", menuName = "Minigames/Safe Code Minigame")]
    public class SafeCodeMinigameScrObj : MinigameScrObj
    {
        public string targetPassword;

        public override void InitMinigame()
        {
            MinigamesManager.Instance.InitSafeCodeMinigame(targetPassword);
        }
    }
}