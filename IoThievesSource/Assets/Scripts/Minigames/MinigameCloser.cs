using UnityEngine;

namespace Minigames
{
    public class MinigameCloser : MonoBehaviour
    {
        [SerializeField] private GameObject minigameWindow;
    
        public void CloseMinigameWindow()
        {
            minigameWindow.SetActive(false);
        }
    }
}
