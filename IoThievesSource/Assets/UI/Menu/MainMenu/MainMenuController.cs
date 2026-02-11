using System;
using Events;
using UnityEngine.UIElements;

public class MainMenuController : MenuController
{
    
    private void OnEnable()
    {
        base.OnEnable();
        var root = menuDocument.rootVisualElement;
        var playButton = root.Q<Button>("playButton");
        playButton.clicked += PlayGame;
        
        EventsManager.OnBackToMainMenu.AddListener(EnableObject);
    }

    private void OnDisable()
    {
        EventsManager.OnBackToMainMenu.RemoveListener(EnableObject);
    }

    private void PlayGame()
    {
        // Go to next scene (Level 1)
        EventsManager.OnEnterLevelSelection?.Invoke();
        
        gameObject.SetActive(false);
    }
    
    private void EnableObject(bool enable)
    {
        gameObject.SetActive(true);
    }
    
}
