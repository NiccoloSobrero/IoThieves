using System;
using Events;
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

    public void PauseMenu()
    {
        Time.timeScale = 0;
        base.menuDocument.enabled = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        // EventsManager.OnResumeGame?.Invoke();
        base.menuDocument.enabled = false;
    }
    
}
