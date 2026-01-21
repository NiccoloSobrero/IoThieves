using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public enum Level
    {
        None = -1,
        Tutorial = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
    
        public Level currentLevel;

        private LevelSelectorManager _levelSelectorManager;
        public LevelSelectorManager LevelSelectorManager
        {
            get
            {
                if (_levelSelectorManager == null)
                {
                    _levelSelectorManager = FindFirstObjectByType<LevelSelectorManager>();
                }
                
                return _levelSelectorManager;
            }
        }
    
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
            EventsManager.OnPlayLevel.AddListener(PlayLevel);
            EventsManager.OnEnterLevelSelection.AddListener(EnterLevelSelection);
            EventsManager.OnSaveGame.AddListener(SaveGame);
            EventsManager.OnPauseGame.AddListener(PauseGame);
            EventsManager.OnResumeGame.AddListener(ResumeGame);
            EventsManager.OnQuitGame.AddListener(QuitGame);
            EventsManager.OnOptionsClicked.AddListener(OpenOptions);
            EventsManager.OnRestartGame.AddListener(RestartGame);
            EventsManager.OnGameOverTriggered.AddListener(GameOver);
            EventsManager.OnNextLevel.AddListener(GoToNextLevel);
            EventsManager.OnBackToMainMenu.AddListener(GoToMainMenu);
        }

        private void OnDisable()
        {
            EventsManager.OnPlayLevel.RemoveListener(PlayLevel);
            EventsManager.OnEnterLevelSelection.RemoveListener(EnterLevelSelection);
            EventsManager.OnSaveGame.RemoveListener(SaveGame);
            EventsManager.OnPauseGame.RemoveListener(PauseGame);
            EventsManager.OnResumeGame.RemoveListener(ResumeGame);
            EventsManager.OnQuitGame.RemoveListener(QuitGame);
            EventsManager.OnOptionsClicked.RemoveListener(OpenOptions);
            EventsManager.OnRestartGame.RemoveListener(RestartGame);
            EventsManager.OnGameOverTriggered.RemoveListener(GameOver);
            EventsManager.OnNextLevel.RemoveListener(GoToNextLevel);
            EventsManager.OnBackToMainMenu.RemoveListener(GoToMainMenu);
        }

        private void PlayLevel(Level level)
        {
            currentLevel = level;
            
            switch (level)
            {
                case Level.Tutorial:
                    SceneManager.LoadScene("TutorialLevel");
                    break;
                case Level.Level1:
                    SceneManager.LoadScene("HR_Level1");
                    break;
                case Level.Level2:
                    SceneManager.LoadScene("Boss_House_Level2");
                    break;
                case Level.Level3:
                    SceneManager.LoadScene("TutorialLevel");
                    break;
                case Level.None:
                default:
                    Debug.LogWarning($"Unknown Level {level}");
                    break;
            }
        }
        
        private void EnterLevelSelection()
        {
            LevelSelectorManager.EnterLevelSelection();
        }

        private void SaveGame()
        {
            // useful if we want to show some loading screen or some feedback to the player
            print("Saving Game");
        }
    
        private void PauseGame()
        {
            print("Pausing Game in GameManager");
            Time.timeScale = 0f;
        }

        private void ResumeGame()
        {
            print("Resuming Game in GameManager");
            Time.timeScale = 1f;
        }

        private void RestartGame()
        {
            print("Restarting Game");
            PlayLevel(currentLevel);
        }

        private void GameOver()
        {
            print("Game Over");
        }

        private void OpenOptions()
        {
            print("Opening Options");
        }
    
        private void QuitGame()
        {
            print("Quitting Game");
# if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
            // EnterLevelSelection();
        }

        private void GoToNextLevel()
        {
            currentLevel++;
            PlayLevel(currentLevel);
        }
    
        public string LoadUserInputPreferences()
        {
            string userInputPreferences = "";
            if (PlayerPrefs.HasKey(Utility.ActionsControl))
            {
                userInputPreferences = PlayerPrefs.GetString(Utility.ActionsControl);
            }

            return userInputPreferences;
        }
    }
}