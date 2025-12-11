using System;
using System.Collections.Generic;
using Events;
using NUnit.Framework;
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

        var moveUpDropdown = root.Q<DropdownField>("moveUpDropdown");
        moveUpDropdown.choices = new List<string>() { KeyCode.W.ToString(), KeyCode.UpArrow.ToString() };
        moveUpDropdown.value = moveUpDropdown.choices[0];
        
        var moveDownDropdown = root.Q<DropdownField>("moveDownDropdown");
        moveDownDropdown.choices = new List<string>() { KeyCode.S.ToString(), KeyCode.DownArrow.ToString() };
        moveDownDropdown.value = moveDownDropdown.choices[0];
        
        var moveLeftDropdown = root.Q<DropdownField>("moveLeftDropdown");
        moveLeftDropdown.choices = new List<string>() { KeyCode.A.ToString(), KeyCode.LeftArrow.ToString() };
        moveLeftDropdown.value = moveLeftDropdown.choices[0];
        
        var moveRightDropdown = root.Q<DropdownField>("moveRightDropdown");
        moveRightDropdown.choices = new List<string>() { KeyCode.D.ToString(), KeyCode.RightArrow.ToString() };
        moveRightDropdown.value = moveRightDropdown.choices[0];
        
        var interactAction = root.Q<DropdownField>("interactDropdown");
        interactAction.choices = new List<string>() { KeyCode.Mouse0.ToString(), KeyCode.KeypadEnter.ToString() };
        interactAction.value = interactAction.choices[0];
        
        var executeAction = root.Q<DropdownField>("executeDropdown");
        executeAction.choices = new List<string>() { KeyCode.Mouse1.ToString(), KeyCode.Return.ToString() };
        executeAction.value = executeAction.choices[0];
        
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
