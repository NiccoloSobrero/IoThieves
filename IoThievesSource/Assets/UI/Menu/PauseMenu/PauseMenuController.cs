using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MenuController
{
    
    private void OnEnable()
    {
        base.OnEnable();
        var root = menuDocument.rootVisualElement;
        var resumeButton = root.Q<Button>("resumeButton");
        resumeButton.clicked += ResumeGame;
    }

    private void ResumeGame()
    {
        print("resume game");
    }
    
}
