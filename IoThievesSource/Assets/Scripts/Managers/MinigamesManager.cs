using System.Collections.Generic;
using Events;
using Interactables;
using Levels;
using Minigames;
using UnityEngine;

namespace Managers
{
    public class MinigamesManager : MonoBehaviour
    {
        public static MinigamesManager Instance;

        [SerializeField] private GameObject minigamesWindow;
        
        private readonly Queue<MinigameAndObjective> _minigamesQueue = new();
        
        private GameObject _currentMinigameObj;
        private MinigameAndObjective _currentMinigameAndObjective;
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
                _currentInteractableToMinigames.TryRemovingMinigame(_currentMinigameAndObjective);
            }

            if (_minigamesQueue.TryDequeue(out _currentMinigameAndObjective))
            {
                _currentMinigameAndObjective.minigameScrObj.InitMinigame(_currentMinigameAndObjective.objectiveScrObj);
            }
            else
            {
                EnableWindow(false);
                
                PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
            }
            
            Debug.Log("Queued:  " + _minigamesQueue.Count);
        }

        private void EnableWindow(bool enable)
        {
            minigamesWindow.SetActive(enable);
        }

        public void SetMinigamesLoop(InteractToMinigames interactable, List<MinigameAndObjective> minigames)
        {
            PlayerInputManager.Instance.SetContext(InputContext.Minigame);
            
            _currentInteractableToMinigames = interactable;
            
            _minigamesQueue.Clear();
            
            EnableWindow(true);
            
            foreach (MinigameAndObjective minigame in minigames)
            {
                _minigamesQueue.Enqueue(minigame);
            }

            MoveToNextMinigame();
        }

        public void CloseMinigameWindow()
        {
            minigamesWindow.SetActive(false);
            _currentMinigameObj.SetActive(false);
            
            _minigamesQueue.Clear();

            _currentMinigameObj = null;
            _currentMinigameAndObjective = null;
            _currentInteractableToMinigames = null;
            
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
        }
        
        #region Minigames Init()
        public void InitPasswordDecryptionMinigame(LevelObjectiveScrObj objectiveScrObj)
        {
            _currentMinigameObj = passwordDecryptionMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            passwordDecryptionMinigame.InitMinigame(objectiveScrObj);
        }

        public void InitDesktopMinigame(FolderContentScrObj folderContentScrObj, LevelObjectiveScrObj objectiveScrObj)
        {
            _currentMinigameObj = desktopMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            desktopMinigame.InitMinigame(folderContentScrObj, objectiveScrObj);
        }
        
        public void InitSafeCodeMinigame(string targetPassword, LevelObjectiveScrObj objectiveScrObj)
        {
            _currentMinigameObj = safeCodeMinigame.gameObject;
            
            _currentMinigameObj.SetActive(true);
            
            safeCodeMinigame.InitMinigame(targetPassword, objectiveScrObj);
        }
        #endregion
    }
}
