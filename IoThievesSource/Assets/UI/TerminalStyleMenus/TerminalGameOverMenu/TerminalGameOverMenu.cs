using Events;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalGameOverMenu : TerminalMenu
{
    protected override void OnMenuEnabled()
    {
        EventsManager.OnGameOverTriggered.AddListener(ShowGameOverMenu);
        HideGameOverMenu();
    }

    private void OnDisable()
    {
        EventsManager.OnGameOverTriggered.RemoveListener(ShowGameOverMenu);
    }

    private void ShowGameOverMenu()
    {
        print("Showing End Level");
        PlayerInputManager.Instance.SetContext(InputContext.Disabled);
        
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        
        PrintLine($"iothieves@kali:~$ {IothievesScan}");
        PrintLine("");
        // PrintLine("IoThieves v0.1.2");
        PrintBanner(Utility.GameOver);
        PrintLine("Copyright (c) 2025");
        PrintLine("");
        
        PrintLine("Available commands:");
        PrintLine(" restart         restart level");
        PrintLine(" help            list all available commands");
        PrintLine(" cd ..           return to main menu");
        // PrintLine(" options         settings [Not Available]");
        PrintLine(" quit            quit game");
    }

    private void HideGameOverMenu()
    {
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
    }
    
    protected override void ProcessCommand(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "help":
                PrintLine("Available commands:");
                PrintLine("restart          restart level");
                PrintLine("help             list all available commands");
                PrintLine("cd ..            return to main menu");
                // PrintLine("options          settings [Not Available]");
                PrintLine("quit             quit game");
                break;
            case "restart":
                PrintLine("Connection established...");
                EventsManager.OnRestartGame?.Invoke();
                gameObject.SetActive(false);
                break;
            case "cd ..":
                PrintLine("Going to main menu...");
                EventsManager.OnBackToMainMenu?.Invoke();
                break;
            // case "options":
            //     PrintLine("Customization session started...");
            //     EventsManager.OnOptionsClicked?.Invoke();
            //     break;
            case "quit":
                PrintLine("Shutting down...");
                EventsManager.OnQuitGame?.Invoke();
                break;
            default:
                PrintLine($"bash: {cmd}: command not found");
                break;
        }
    }
}
