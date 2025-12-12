using System;
using System.Collections.Generic;
using Events;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument optionsMenuDocument;
    [SerializeField] private InputActionAsset inputActions;

    private InputAction move;

    private void Awake()
    {
        var gameplay = inputActions.FindActionMap("Gameplay");

        move = gameplay.FindAction("Move");
    }

    private void RebindButton(Button button, InputAction action, string partName)
    {
        button.text = GetDisplayName(action, partName);

        button.clicked += () =>
        {
            RebindAction(button, action, partName);
        };
    }

    private void RebindAction(Button button, InputAction action, string partName)
    {
        int bindingIndex = FindCompositePart(action, "WASD", partName);
        
        button.text = "Press a key...";
        button.SetEnabled(false);

        
        action.Disable();

        action.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<keyboard>/escape")
            .OnCancel(op =>
            {
                button.text = GetDisplayName(action, partName);
                button.SetEnabled(true);
                op.Dispose();
                action.Enable();
            })
            .OnComplete(op =>
            {
                button.text = GetDisplayName(action, partName);
                button.SetEnabled(true);
                op.Dispose();
                action.Enable();
            })
            .Start();
    }

    private int FindCompositePart(InputAction action, string compositeName, string partName)
    {
        bool isInsideComposite = false;
        for (int index = 0; index < action.bindings.Count; index++)
        {
            var binding = action.bindings[index];

            if (binding.isComposite && binding.name == compositeName)
            {
                isInsideComposite = true;
                continue;
            }

            if (binding.isComposite)
            {
                isInsideComposite = false;
            }

            if (isInsideComposite && binding.isPartOfComposite &&
                binding.name.Equals(partName, StringComparison.OrdinalIgnoreCase))
            {
                return index;
            }
        }

        throw new Exception($"Composite {compositeName} with part {partName} not found");
    }

    private string GetDisplayName(InputAction action, string partName)
    {
        int index = FindCompositePart(action, "WASD", partName);
        return action.GetBindingDisplayString(index);
    }

    private void OnEnable()
    {
        var root = optionsMenuDocument.rootVisualElement;
        var backButton = root.Q<Button>("backButton");
        backButton.clicked += HideOptionsMenu;
        
        var saveButton = root.Q<Button>("saveButton");
        saveButton.clicked += SaveOptions;

        // var moveUpDropdown = root.Q<DropdownField>("moveUpDropdown");
        // moveUpDropdown.choices = new List<string>() { KeyCode.W.ToString(), KeyCode.UpArrow.ToString() };
        // moveUpDropdown.value = moveUpDropdown.choices[0];

        var moveUpButton = root.Q<Button>("moveUpButton");
        moveUpButton.clicked += () =>
        {
            RebindButton(moveUpButton, move, "Up");
        };
        
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
