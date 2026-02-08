using System;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using PlayerInputManager = Managers.PlayerInputManager;

public class TerminalPauseMenu : TerminalMenu
{
    [SerializeField] private InputActionAsset inputActions;

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
                PrintColumns(" bind [move-up, move-down, move-left, move-right, click, throw]", "change button binding");
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
                EventsManager.OnResumeGame?.Invoke();
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
        print("Showing Pause Menu");
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
        PrintLine("     resume                                                                  resume game");
        PrintLine("     bindings                                                                print all bindings");
        PrintLine("     bind [move-up, move-down, move-left, move-right, click, throw]          change button binding");
        PrintLine("     cd ..                                                                   return to main menu");
        PrintLine("     quit                                                                    quit game");
        
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
        // example: bind move-up
        string[] parts = cmd.Split(' ');

        if (parts.Length != 2)
        {
            PrintLine("Usage: bind <action>");
            return;
        }

        string action = parts[1];

        PrintLine("Press a key to bind (ESC to cancel)");

        if (action == "move-up")
        {
            GetIndexAndStartRebind(Utility.GameplayAction, Utility.MoveAction, Utility.UpAction, HandleBindResult);
        } else if (action == "move-down") {
            GetIndexAndStartRebind(Utility.GameplayAction, Utility.MoveAction, Utility.DownAction, HandleBindResult);
        } else if (action == "move-left")
        {
            GetIndexAndStartRebind(Utility.GameplayAction, Utility.MoveAction, Utility.LeftAction, HandleBindResult);
        } else if (action == "move-right")
        {
            GetIndexAndStartRebind(Utility.GameplayAction, Utility.MoveAction, Utility.RightAction, HandleBindResult);
        } else if (action == "click")
        {
            StartSimpleRebind(Utility.GameplayAction, Utility.ClickAction, action, HandleBindResult);
        } else if (action == "throw")
        {
            StartSimpleRebind(Utility.GameplayAction, Utility.ThrowAction, action, HandleBindResult);
        }
    }

    private void GetIndexAndStartRebind(string actionMap, string actionName, string part, Action<string, string> action)
    {
        int index = GetCompositeIndex(actionName, part);
        StartRebind(actionMap, actionName, index, action, part);
    }

    private void StartSimpleRebind(string actionMap, string actionName, string part, Action<string, string> action)
    {
        StartRebind(actionMap, actionName, 0, action, part);
    }
    
    private void HandleBindResult(string result, string partAction)
    {
        Debug.Log($"Bound {partAction} to {result}");
        PrintLine($"Bound {partAction} to {result}");
    }
    
    public void StartRebind(
        string actionMap,
        string actionName,
        int bindingIndex,
        Action<string, string> onComplete,
        string part)
    {
        UIInputMode.IsTerminalInputEnabled = false;
        UIInputMode.IsRebinding = true;
    
        var map = inputActions.FindActionMap(actionMap);
        var currentAction = map.FindAction(actionName);
    
        currentAction.Disable();
    
        currentAction.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough(Utility.KeyboardEscape)
            .OnCancel(op => EndRebind("Cancelled", currentAction, null, string.Empty))
            .OnComplete(op =>
            {
                op.Dispose();
                currentAction.Enable();
                string result = currentAction.bindings[bindingIndex].ToDisplayString();
                SaveActionsRebind();
                EndRebind(result, currentAction, onComplete, part);
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

            if (b.isComposite && b.name == Utility.WasdAction)
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

    private void EndRebind(string result, InputAction currentAction, Action<string, string> onComplete, string partAction)
    {
        currentAction.Enable();
        currentAction.actionMap.Enable();
        UIInputMode.IsRebinding = false;
        UIInputMode.IsTerminalInputEnabled = true;
    
        onComplete?.Invoke(result, partAction);
        onComplete = null;
    
        SaveActionsRebind();
    }
    
    private void SaveActionsRebind()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(Utility.ActionsControl, rebinds);
        PlayerPrefs.Save();
        print("Saving...");
        EventsManager.OnOptionsSave?.Invoke();
    }

    private void LoadUserInputPreferences()
    {
        if (PlayerPrefs.HasKey(Utility.ActionsControl))
        {
            string userInputPreferences = PlayerPrefs.GetString(Utility.ActionsControl);
            inputActions.LoadBindingOverridesFromJson(userInputPreferences);
            print("User input preferences loaded");
        }
    }
    
    
}
