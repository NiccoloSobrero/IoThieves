using System;
using Events;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    
    protected void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        var saveButton = root.Q<Button>("saveButton");
        saveButton.SetEnabled(false);
        var optionsButton = root.Q<Button>("optionsButton");
        // optionsButton.SetEnabled(false);
        var quitButton = root.Q<Button>("quitButton");
        if (saveButton != null)
        {
            saveButton.clicked += SaveGame;
        }
        optionsButton.clicked += OpenGameOptions;
        quitButton.clicked += QuitGame;
    }

    private void SaveGame()
    {
        EventsManager.OnSaveGame?.Invoke();
    }
    
    private void OpenGameOptions()
    {
        EventsManager.OnOptionsClicked?.Invoke();
    }
    
    private void QuitGame()
    {
        EventsManager.OnQuitGame?.Invoke();
    }
}
