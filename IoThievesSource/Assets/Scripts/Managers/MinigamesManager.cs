using System.Collections.Generic;
using Events;
using Interactables;
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
        private InteractToMinigames _currentInteractableToMinigames;
        
        [Header("Minigames")]
        [SerializeField] private PasswordDecryptionMinigame passwordDecryptionMinigame;
        [SerializeField] private DesktopMinigame desktopMinigame;
        [SerializeField] private SafeCodeMinigame safeCodeMinigame;

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

            if (_currentInteractableToMinigames != null)
            {
                _currentInteractableToMinigames.TryRemovingMinigame(_currentMinigameScrObj);
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

        public void SetMinigamesLoop(InteractToMinigames interactable, List<MinigameScrObj> minigames)
        {
            PlayerInputManager.Instance.SetContext(InputContext.Minigame);
            
            _currentInteractableToMinigames = interactable;
            
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
            _currentInteractableToMinigames = null;
            
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
        }


        public void InitSafeCodeMinigame(string targetPassword)
        {
            _currentMinigameObj = safeCodeMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            safeCodeMinigame.InitMinigame(targetPassword);
        }
    }
}
