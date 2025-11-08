using UnityEngine;

namespace Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
        public abstract void InitMinigame();
        
        public abstract bool CheckCompletion();
    }
}
