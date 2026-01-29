using System;
using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [Serializable]
        private class EnemyToPos
        {
            public GameObject enemy;
            public Vector3 position;
        }
        
        [Header("Restart")]
        [SerializeField] protected Transform playerTransform;
        [SerializeField] private List<GameObject> enemies = new();
        private readonly List<EnemyToPos> _enemiesStartPos = new();

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

            foreach (GameObject enemy in enemies)
            {
                _enemiesStartPos.Add(new EnemyToPos
                {
                    enemy = enemy,
                    position = enemy.transform.position
                });
            }
        }

        protected virtual void RestartLevel()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
            
            playerTransform.position = _lastValidPosition;

            foreach (EnemyToPos enemy in _enemiesStartPos)
            {
                enemy.enemy.transform.position = enemy.position;
            }
        }
        
        protected void SaveLastValidPosition()
        {
            _lastValidPosition = playerTransform.position;
        }
    }
}
