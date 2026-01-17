using System;
using System.Linq;
using Events;
using Minigames;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using PlayerInputManager = Managers.PlayerInputManager;

public class TerminalPauseMenu : TerminalMenu
{
    [SerializeField] private InputActionAsset inputActions;
    
    // private InputAction _currentAction;
    private int _bindingIndex;
    // private Action<string> _onComplete;
    
    private InputAction move;
    private InputAction interact;
    private InputAction execute;
    
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

        // LoadUserInputPreferences();
    }

    protected override void OnMenuEnabled()
    {
        EventsManager.OnPauseGame.AddListener(ShowPauseMenu);
        HidePauseMenu();
    }

    private void OnDisable()
    {
        EventsManager.OnPauseGame.RemoveListener(ShowPauseMenu);
    }
    
    protected override void ProcessCommand(string cmd)
    {
        cmd = cmd.Trim();

        if (cmd.StartsWith("bind "))
        {
            HandleBindCommand(cmd);
            return;
        }
        switch (cmd.ToLower())
        {
            case "help":
                PrintLine("Available commands:");
                PrintColumns(" resume", "resume game");
                PrintColumns(" bindings", "print all bindings");
                PrintColumns(" cd ..", "return to main menu");
                PrintColumns(" quit", "quit game");
                break;
            case "resume":
                PrintLine("Resuming game...");
                HidePauseMenu();
                EventsManager.OnResumeGame?.Invoke();
                break;
            case "bindings":
                PrintBindings("Gameplay", "Move");
                break;
            case "cd ..":
                PrintLine("Going to main menu...");
                EventsManager.OnBackToMainMenu?.Invoke();
                break;
            case "quit":
                PrintLine("Shutting down...");
                EventsManager.OnQuitGame?.Invoke();
                break;
            default:
                PrintLine($"bash: {cmd}: command not found");
                break;
        }
    }
    
    void PrintBindings(string mapName, string actionName)
    {
        var action = inputActions.FindActionMap(mapName).FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            PrintLine($"{i}: {b.path} ({b.ToDisplayString()})");
        }
    }

    
    private void ShowPauseMenu()
    {
        print("Showing End Level");
        PlayerInputManager.Instance.SetContext(InputContext.Disabled);
        
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        
        PrintLine("");
        PrintLine("IoThieves v0.1.2");
        PrintLine("Copyright (c) 2025");
        // PrintLine("Polimi Game Collective");
        PrintBanner(Utility.PauseArt);
        PrintLine("");
        
        // PrintLine("Type 'iothieves --help' to list commands");
        PrintLine("Available commands:");
        PrintLine("     resume               resume game");
        PrintLine("     bindings             print all bindings");
        PrintLine("     bind [move.up]       change button binding");
        PrintLine("     cd ..                return to main menu");
        PrintLine("     quit                 quit game");
        
        PrintLine("");
            
        PrintLine("Click anywhere to start hacking...");
    }
    
    private void HidePauseMenu()
    {
        ClearScrollView();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
    }

    private void HandleBindCommand(string cmd)
    {
        // example: bind move.up
        string[] parts = cmd.Split(' ');

        if (parts.Length != 2)
        {
            PrintLine("Usage: bind <action>");
            return;
        }

        string action = parts[1];

        PrintLine("Press a key to bind (ESC to cancel)");

        if (action == "move.up")
        {
            int index = GetCompositeIndex("Move", "Up");
        
            Debug.Log($"[Rebind] Binding index = {index}");
            StartRebind(
                "Gameplay",
                "Move",
                index,
                result =>
                {
                    Debug.Log($"Bound move.up to {result}");
                    PrintLine($"Bound move.up to {result}");
                });
            // RebindComposite(move, WASD_ACTION, "Up");
        } else if (action == "move.down") {
            int index = GetCompositeIndex("Move", "Down");
        
            Debug.Log($"[Rebind] Binding index = {index}");
            StartRebind(
                "Gameplay",
                "Move",
                index,
                result =>
                {
                    Debug.Log($"Bound move.down to {result}");
                    PrintLine($"Bound move.down to {result}");
                });
        } else if (action == "move.left")
        {
            int index = GetCompositeIndex("Move", "Left");
            Debug.Log($"[Rebind] Binding index = {index}");
            StartRebind(
                "Gameplay",
                "Move",
                index,
                result =>
                {
                    Debug.Log($"Bound move.down to {result}");
                    PrintLine($"Bound move.down to {result}");
                });
        } else if (action == "move.right")
        {
            int index = GetCompositeIndex("Move", "Right");
            Debug.Log($"[Rebind] Binding index = {index}");
            StartRebind(
                "Gameplay",
                "Move",
                index,
                result =>
                {
                    Debug.Log($"Bound move.down to {result}");
                    PrintLine($"Bound move.down to {result}");
                });
        }
    }
    
    public void StartRebind(
        string actionMap,
        string actionName,
        int bindingIndex,
        Action<string> onComplete)
    {
        UIInputMode.IsTerminalInputEnabled = false;
        UIInputMode.IsRebinding = true;
    
        var map = inputActions.FindActionMap(actionMap);
        var currentAction = map.FindAction(actionName);
        _bindingIndex = bindingIndex;
        // _onComplete = onComplete;
    
        currentAction.Disable();
    
        currentAction.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<keyboard>/escape")
            .OnCancel(op => EndRebind("Cancelled", currentAction, null))
            .OnComplete(op =>
            {
                op.Dispose();
                currentAction.Enable();
                string result = currentAction.bindings[bindingIndex].ToDisplayString();
                SaveActionsRebind();
                EndRebind(result, currentAction, onComplete);
            })
            .Start();
    }
    
    public int GetCompositeIndex(string actionName, string part)
    {
        var action = inputActions.FindAction(actionName);

        bool inside = false;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];

            if (b.isComposite && b.name == "WASD")
            {
                inside = true;
                continue;
            }

            if (inside && b.isPartOfComposite &&
                b.name.Equals(part, StringComparison.OrdinalIgnoreCase))
                return i;

            if (b.isComposite)
                inside = false;
        }

        return -1;
    }

    private void EndRebind(string result, InputAction currentAction, Action<string> onComplete)
    {
        currentAction.Enable();
        currentAction.actionMap.Enable();
        UIInputMode.IsRebinding = false;
        UIInputMode.IsTerminalInputEnabled = true;
    
        onComplete?.Invoke(result);
        onComplete = null;
    
        SaveActionsRebind();
    }
    
    private void SaveActionsRebind()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("ActionsControl", rebinds);
        PlayerPrefs.Save();
        print("Saving...");
        EventsManager.OnOptionsSave?.Invoke();
    }

    private void LoadUserInputPreferences()
    {
        if (PlayerPrefs.HasKey("ActionsControl"))
        {
            string userInputPreferences = PlayerPrefs.GetString("ActionsControl");
            inputActions.LoadBindingOverridesFromJson(userInputPreferences);
            print("User input preferences loaded");
        }
    }
    
    
}
