using System;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
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
        EventsManager.OnStartGame.AddListener(StartGame);
        EventsManager.OnSaveGame.AddListener(SaveGame);
        EventsManager.OnPauseGame.AddListener(PauseGame);
        EventsManager.OnResumeGame.AddListener(ResumeGame);
        EventsManager.OnQuitGame.AddListener(QuitGame);
        EventsManager.OnOptionsClicked.AddListener(OpenOptions);
        EventsManager.OnRestartGame.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        EventsManager.OnStartGame.RemoveListener(StartGame);
        EventsManager.OnSaveGame.RemoveListener(SaveGame);
        EventsManager.OnPauseGame.RemoveListener(PauseGame);
        EventsManager.OnResumeGame.RemoveListener(ResumeGame);
        EventsManager.OnQuitGame.RemoveListener(QuitGame);
        EventsManager.OnOptionsClicked.RemoveListener(OpenOptions);
        EventsManager.OnRestartGame.RemoveListener(RestartGame);
    }

    private void StartGame()
    {
        // load correct level
        print("Starting Game");

        SceneManager.LoadScene("TutorialLevel");
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
    
    public string LoadUserInputPreferences()
    {
        string userInputPreferences = "";
        if (PlayerPrefs.HasKey("ActionsControl"))
        {
            userInputPreferences = PlayerPrefs.GetString("ActionsControl");
        }

        return userInputPreferences;
    }
}
