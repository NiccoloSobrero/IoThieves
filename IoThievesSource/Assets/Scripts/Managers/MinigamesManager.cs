using System.Collections.Generic;
using Events;
using Minigames;
using Minigames.ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class MinigamesManager : MonoBehaviour
    {
        public static MinigamesManager Instance;

        [SerializeField] private GameObject minigamesWindow;
        [SerializeField] private Transform minigamesParent;

        public Transform MinigamesParent => minigamesParent;
        
        public readonly Queue<MinigameScrObj> MinigamesQueue = new();
        private GameObject _currentMinigameObj;
        
        [Header("Minigames")]
        [SerializeField] private PasswordDecryptorManager passwordDecryptionMinigame;
        [SerializeField] private DesktopMinigame desktopMinigame;

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
        
        private void OnEnable()
        {
            EventsManager.OnMinigameCompleted.AddListener(MoveToNextMinigame);
        }

        private void OnDisable()
        {
            EventsManager.OnMinigameCompleted.RemoveListener(MoveToNextMinigame);
        }

        public void MoveToNextMinigame()
        {
            if (_currentMinigameObj != null)
            {
                _currentMinigameObj.SetActive(false);
            }
            
            MinigamesQueue.Dequeue().InitMinigame();
        }

        public void EnableWindow(bool enable)
        {
            minigamesWindow.SetActive(enable);
        }

        public void InitPasswordDecryptionMinigame()
        {
            _currentMinigameObj = passwordDecryptionMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            passwordDecryptionMinigame.InitGame();
        }

        public void InitDesktopMinigame(FolderContentScrObj folderContentScrObj)
        {
            _currentMinigameObj = desktopMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            desktopMinigame.OpenDesktop(folderContentScrObj);
        }
    }
}
