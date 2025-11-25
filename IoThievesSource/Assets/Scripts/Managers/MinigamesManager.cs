using System.Collections.Generic;
using Events;
using Hackables;
using Minigames;
using Minigames.ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class MinigamesManager : MonoBehaviour
    {
        public static MinigamesManager Instance;

        [SerializeField] private GameObject minigamesWindow;
        
        private readonly Queue<MinigameScrObj> _minigamesQueue = new();
        
        private GameObject _currentMinigameObj;
        private MinigameScrObj _currentMinigameScrObj;
        private HackableWithMinigames _currentHackedMinigame;
        
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

        private void MoveToNextMinigame()
        {
            if (_currentMinigameObj != null)
            {
                _currentMinigameObj.SetActive(false);
            }

            if (_currentHackedMinigame != null)
            {
                _currentHackedMinigame.TryRemovingMinigame(_currentMinigameScrObj);
            }
            
            _currentMinigameScrObj = _minigamesQueue.Dequeue();
            _currentMinigameScrObj.InitMinigame();
        }

        private void EnableWindow(bool enable)
        {
            minigamesWindow.SetActive(enable);
        }

        public void InitPasswordDecryptionMinigame()
        {
            _currentMinigameObj = passwordDecryptionMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            passwordDecryptionMinigame.InitMinigame();
        }

        public void InitDesktopMinigame(FolderContentScrObj folderContentScrObj)
        {
            _currentMinigameObj = desktopMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            desktopMinigame.InitMinigame(folderContentScrObj);
        }

        public void SetMinigamesLoop(HackableWithMinigames hackable, List<MinigameScrObj> minigames)
        {
            PlayerInputManager.Instance.SetContext(InputContext.Minigame);
            
            _currentHackedMinigame = hackable;
            
            _minigamesQueue.Clear();
            
            EnableWindow(true);
            
            foreach (MinigameScrObj minigameScrObj in minigames)
            {
                _minigamesQueue.Enqueue(minigameScrObj);
            }

            MoveToNextMinigame();
        }

        public void CloseMinigameWindow()
        {
            minigamesWindow.SetActive(false);
            _currentMinigameObj.SetActive(false);
            
            _minigamesQueue.Clear();

            _currentMinigameObj = null;
            _currentMinigameScrObj = null;
            _currentHackedMinigame = null;
            
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
        }
    }
}
