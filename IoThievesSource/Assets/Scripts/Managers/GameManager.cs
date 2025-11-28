using System;
using Events;
using UnityEngine;

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
        EventsManager.OnResumeGame.AddListener(ResumeGame);
        EventsManager.OnQuitGame.AddListener(QuitGame);
        EventsManager.OnOptionsClicked.AddListener(OpenOptions);
        EventsManager.OnRestartGame.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        EventsManager.OnStartGame.RemoveListener(StartGame);
        EventsManager.OnSaveGame.RemoveListener(SaveGame);
        EventsManager.OnResumeGame.RemoveListener(ResumeGame);
        EventsManager.OnQuitGame.RemoveListener(QuitGame);
        EventsManager.OnOptionsClicked.RemoveListener(OpenOptions);
        EventsManager.OnRestartGame.RemoveListener(RestartGame);
    }

    private void StartGame()
    {
        // load correct level
        print("Starting Game");
    }

    private void SaveGame()
    {
        // useful if we want to show some loading screen or some feedback to the player
        print("Saving Game");
    }

    private void ResumeGame()
    {
        print("Resuming Game");
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
