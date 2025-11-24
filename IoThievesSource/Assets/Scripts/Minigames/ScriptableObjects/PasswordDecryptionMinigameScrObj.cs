using Managers;
using UnityEngine;

namespace Minigames.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PasswordDecryptionMinigame", menuName = "Minigames/Password decryption minigame")]
    public class PasswordDecryptionMinigameScrObj : MinigameScrObj
    {
        public override void InitMinigame()
        {
            MinigamesManager.Instance.InitPasswordDecryptionMinigame();
        }
    }
}