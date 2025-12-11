using System;
using Events;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument optionsMenuDocument;
    private void OnEnable()
    {
        var root = optionsMenuDocument.rootVisualElement;
        var backButton = root.Q<Button>("backButton");
        backButton.clicked += HideOptionsMenu;
        
        var saveButton = root.Q<Button>("saveButton");
        saveButton.clicked += SaveOptions;
        
        // EventsManager.OnPauseGame.AddListener(SaveOptions);
        EventsManager.OnOptionsClicked.AddListener(ShowMenuOptions);
        // EventsManager.OnResumeGame.AddListener(ResumeGame);
        HideOptionsMenu();
    }

    private void OnDisable()
    {
        EventsManager.OnOptionsClicked.RemoveListener(ShowMenuOptions);
    }

    private void ShowMenuOptions()
    {
        optionsMenuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void HideOptionsMenu()
    {
        print("Hide Options Menu");
        optionsMenuDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void SaveOptions()
    {
        EventsManager.OnOptionsSave?.Invoke();
        HideOptionsMenu();
    }
}
