using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private UIDocument pauseMenuDocument;
    
    private void OnEnable()
    {
        var root = pauseMenuDocument.rootVisualElement;
        var resumeButton = root.Q<Button>("resumeButton");
        var saveButton = root.Q<Button>("saveButton");
        var optionsButton = root.Q<Button>("optionsButton");
        var quitButton = root.Q<Button>("quitButton");
        resumeButton.clicked += ResumeGame;
        saveButton.clicked += SaveGame;
        optionsButton.clicked += OpenGameOptions;
        quitButton.clicked += QuitGame;
    }

    private void ResumeGame()
    {
        print("resume game");
    }
    
    private void SaveGame()
    {
        print("save game");
    }
    
    private void OpenGameOptions()
    {
        print("open game options");
    }
    
    private void QuitGame()
    {
        print("quit game");
    }
}
