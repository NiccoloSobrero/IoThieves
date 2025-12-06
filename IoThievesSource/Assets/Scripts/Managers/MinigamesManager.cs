using System.Collections.Generic;
using Events;
using Interactables;
using Levels;
using Minigames;
using Minigames.ScriptableObjects;
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

        private MinigameScrObjToObjective _desktopMinigameOpenObjective;
        private MinigameScrObjToObjective _passwordDecryptionMinigameOpenObjective;

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
            Debug.Log("Called MoveToNextMinigame");
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

                if (_desktopMinigameOpenObjective != null &&
                    _currentMinigameAndObjective.minigameScrObj == _desktopMinigameOpenObjective.MinigameScrObj)
                {
                    _desktopMinigameOpenObjective.Objective.Complete();

                    _desktopMinigameOpenObjective = null;
                }
                else if (_passwordDecryptionMinigameOpenObjective != null &&
                    _currentMinigameAndObjective.minigameScrObj == _passwordDecryptionMinigameOpenObjective.MinigameScrObj)
                {
                    _passwordDecryptionMinigameOpenObjective.Objective.Complete();

                    _passwordDecryptionMinigameOpenObjective = null;
                }
            }
            else
            {
                EnableWindow(false);
                
                PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
            }
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

        #region Minigames objective initialization

        public void SetDesktopMinigameOpenObjective(MinigameScrObj minigameScrObj, TutorialObjective objective)
        {
            _desktopMinigameOpenObjective = new MinigameScrObjToObjective(minigameScrObj, objective);
        }
        
        public void SetPasswordDecryptionMinigameOpenObjective(MinigameScrObj minigameScrObj, TutorialObjective objective)
        {
            _passwordDecryptionMinigameOpenObjective = new MinigameScrObjToObjective(minigameScrObj, objective);
        }

        #endregion
    }
    
    public class MinigameScrObjToObjective
    {
        public readonly MinigameScrObj MinigameScrObj;
        public readonly TutorialObjective Objective;

        public MinigameScrObjToObjective(MinigameScrObj minigameScrObj, TutorialObjective objective)
        {
            MinigameScrObj = minigameScrObj;
            Objective = objective;
        }
    }
}
