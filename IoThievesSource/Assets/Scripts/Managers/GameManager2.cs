using UnityEngine;

namespace Managers
{
    public class GameManager2 : MonoBehaviour
    {
        public static GameManager2 Instance;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TriggerGameOver()
        {
            Debug.Log("Game Over");
        
        }
    }
}
