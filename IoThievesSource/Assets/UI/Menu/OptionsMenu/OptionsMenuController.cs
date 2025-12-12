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
    private InputAction interact;
    private InputAction execute;

    private string previousButtonLabel = string.Empty;

    private readonly string MOVE_ACTION = "Move";
    private readonly string CLICK_ACTION = "Click";
    private readonly string THROW_ACTION = "Throw";
    private readonly string WASD_ACTION = "WASD";
    

    private void Awake()
    {
        var gameplay = inputActions.FindActionMap("Gameplay");

        move = gameplay.FindAction(MOVE_ACTION);

        interact = gameplay.FindAction(CLICK_ACTION);
        execute = gameplay.FindAction(THROW_ACTION);
    }

    private void OnEnable()
    {
        var root = optionsMenuDocument.rootVisualElement;
        var backButton = root.Q<Button>("backButton");
        backButton.clicked += HideOptionsMenu;
        
        var saveButton = root.Q<Button>("saveButton");
        saveButton.clicked += SaveOptions;

        var moveUpButton = root.Q<Button>("moveUpButton");
        moveUpButton.clicked += () =>
        {
            RebindComposite(move, moveUpButton, WASD_ACTION, "Up");
        };

        var moveDownButton = root.Q<Button>("moveDownButton");
        moveDownButton.clicked += () =>
        {
            RebindComposite(move, moveDownButton, WASD_ACTION, "Down");
        };

        var moveLeftButton = root.Q<Button>("moveLeftButton");
        moveLeftButton.clicked += () =>
        {
            RebindComposite(move, moveLeftButton, WASD_ACTION, "Left");
        };

        var moveRightButton = root.Q<Button>("moveRightButton");
        moveRightButton.clicked += () =>
        {
            RebindComposite(move, moveRightButton, WASD_ACTION, "Right");
        };

        var interactButton = root.Q<Button>("interactButton");
        interactButton.clicked += () =>
        {
            RebindSimple(interact, interactButton);
        };
        
        var executeButton = root.Q<Button>("executeButton");
        executeButton.clicked += () =>
        {
            RebindSimple(execute, executeButton);
        };
        
        EventsManager.OnOptionsClicked.AddListener(ShowMenuOptions);
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
        optionsMenuDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void SaveOptions()
    {
        EventsManager.OnOptionsSave?.Invoke();
        HideOptionsMenu();
    }

    private void RebindSimple(InputAction action, Button button)
    {
        int bindingIndex = 0;
        Rebind(action, button, bindingIndex);
    }

    private void RebindComposite(InputAction action, Button button, string compositeName, string partName)
    {
        int bindingIndex = GetCompositeBindingIndex(action, compositeName, partName);
        
        Rebind(action, button, bindingIndex);
    }

    private int GetCompositeBindingIndex(InputAction action, string compositeName, string partName)
    {
        bool isInsideComposite = false;
        int bindingIndex = -1;
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
                bindingIndex = index;
            }

        }
        return bindingIndex;
    }

    private void Rebind(InputAction action, Button button, int index)
    {
        previousButtonLabel = button.text;
        button.text = "Press a key...";
        button.SetEnabled(false);

        
        action.Disable();

        action.PerformInteractiveRebinding(index)
            .WithCancelingThrough("<keyboard>/escape")
            .OnCancel(op =>
            {
                button.text = previousButtonLabel;
                button.SetEnabled(true);
                op.Dispose();
                action.Enable();
            })
            .OnComplete(op =>
            {
                button.text = action.bindings[index].ToDisplayString();
                button.SetEnabled(true);
                op.Dispose();
                action.Enable();
            })
            .Start();
    }
}
