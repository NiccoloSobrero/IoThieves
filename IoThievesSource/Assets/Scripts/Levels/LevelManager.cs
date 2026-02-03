using Events;
using Managers;
using UnityEngine;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Restart")]
        [SerializeField] protected Transform playerTransform;

        private Vector3 _lastValidPosition;

        private void OnEnable()
        {
            EventsManager.OnRestartGame.AddListener(RestartLevel);
        }

        private void OnDisable()
        {
            EventsManager.OnRestartGame.RemoveListener(RestartLevel);
        }
        
        protected virtual void Start()
        {
            SaveLastValidPosition();
        }

        protected virtual void RestartLevel()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
            
            playerTransform.position = _lastValidPosition;
        }
        
        protected void SaveLastValidPosition()
        {
            _lastValidPosition = playerTransform.position;
        }
    }
}
