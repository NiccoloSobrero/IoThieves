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
        
        EventsManager.OnPauseGame.AddListener(PauseMenu);
        // EventsManager.OnResumeGame.AddListener(ResumeGame);
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        EventsManager.OnPauseGame.RemoveListener(PauseMenu);
        // EventsManager.OnResumeGame.RemoveListener(ResumeGame);
    }

    public void PauseMenu()
    {
        print("calling pause in Pause Controller");
        // Time.timeScale = 0;
        menuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void ResumeGame()
    {
        // Time.timeScale = 1;
        print("resuming in pause controller");
        // base.menuDocument.enabled = false;
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
        EventsManager.OnResumeGame?.Invoke();
    }
    
}
