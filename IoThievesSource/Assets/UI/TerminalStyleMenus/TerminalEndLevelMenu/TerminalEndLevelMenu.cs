using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Managers;
using Minigames;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalEndLevelMenu : TerminalMenu
{
    // for testing purposes
    // [SerializeField]
    // private List<FileDetails> _downloadedFile = new List<FileDetails>();
    
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
        string command = string.Empty;
        string fileName = string.Empty;
        if (cmd.Length > 2 && cmd.Contains("cat"))
        {
            command = cmd.Substring(0, 3);
            fileName = cmd.Substring(3).Trim();
            print("file: " + fileName);
            print(cmd);
        }
        string detectCmd = command == string.Empty ? cmd : command;
        switch (detectCmd.ToLower())
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
                // foreach (FileDetails document in _downloadedFile)
                // {
                    PrintLine(document.FileName);
                }
                break;
            case "cat":
                PrintLine("Open the specified document: " + fileName);
                foreach (FileDetails document in DownloadsManager.Instance.DownloadedFiles
                             .Where(file => file.levelOfDownload == GameManager.Instance.currentLevel))
                {
                // foreach (FileDetails document in _downloadedFile)
                // {
                    if (document.FileName == fileName)
                    {
                        Image image = new Image();
                        image.sprite = document.documentSprite;
                        image.AddToClassList("image");
                        _output.Add(image);
                    }
                }
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
