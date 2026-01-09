using System;
using System.Linq;
using Events;
using Managers;
using Minigames;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalEndLevelMenu : TerminalMenu
{
    
    protected override void OnMenuEnabled()
    {
        EventsManager.OnEndLevel.AddListener(ShowEndLevelMenu);
        HideEndLevelMenu();
    }

    private void OnDisable()
    {
        EventsManager.OnEndLevel.RemoveListener(ShowEndLevelMenu);
    }

    protected override void ProcessCommand(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "iothieves --help":
                PrintLine("Available commands: ls, cat [FILENAME], iothieves --scan --next-target, iothieves --return, iothieves --exit");
                break;
            case "iothieves --scan --next-target":
                PrintLine("Exploiting next target...");
                EventsManager.OnNextLevel?.Invoke();
                break;
            case "iothieves --return":
                PrintLine("Going to main menu...");
                EventsManager.OnBackToMainMenu?.Invoke();
                break;
            case "iothieves --exit":
                PrintLine("Shutting down...");
                EventsManager.OnQuitGame?.Invoke();
                break;
            case "ls":
                PrintLine("Printing all the available documents:");
                PrintLine("");
                print("current level: " + GameManager.Instance.currentLevel);
                foreach (FileDetails document in DownloadsManager.Instance.DownloadedFiles
                             .Where(file => file.levelOfDownload == GameManager.Instance.currentLevel))
                {
                    PrintLine(document.filePath);
                }
                break;
            case "cat":
                PrintLine("Open the specified document");
                break;
            default:
                PrintLine($"bash: {cmd}: command not found");
                break;
        }
    }
    
    private void ShowEndLevelMenu()
    {
        print("Showing End Level");
        PlayerInputManager.Instance.SetContext(InputContext.Disabled);
        
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        
        PrintLine("");
        PrintLine("IoThieves v0.1.2");
        PrintLine("Copyright (c) 2025 by IoTeam");
        PrintLine("Polimi Game Collective");
        PrintLine("");
        
        PrintLine("Type 'iothieves --help' to list commands");
    }
    
    private void HideEndLevelMenu()
    {
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
    }
}
