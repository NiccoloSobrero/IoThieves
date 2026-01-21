using System.Linq;
using Events;
using Managers;
using Minigames;
using UnityEngine.UIElements;

public class TerminalCreditMenu : TerminalMenu
{
    
    protected override void OnMenuEnabled()
    {
        EventsManager.OnGameCompleted.AddListener(ShowCreditMenu);
        HideCreditMenu();
    }

    private void OnDisable()
    {
        EventsManager.OnGameCompleted.RemoveListener(ShowCreditMenu);
    }
    
    private void ShowCreditMenu()
    {
        PlayerInputManager.Instance.SetContext(InputContext.Disabled);
        
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        var menu = uiDocument.rootVisualElement.Q<VisualElement>("terminalMainMenu");
        menu.schedule.Execute(() =>
        {
            menu.AddToClassList("visible");
        });
        
        PrintLine("");
        PrintLine("IoThieves v0.1.2");
        PrintLine("Copyright (c) 2025");
        // PrintLine("Polimi Game Collective");
        PrintBanner(Utility.CreditArt);
        PrintLine("");
        
        PrintLine("Thank you for playing IoThieves!");
        PrintLine("We really appreciate your time and support.");
        PrintLine("We’re continuing development with new levels planned.");
        PrintLine("Leave us your feedback and help shape the game!");
        
        PrintLine("Available commands:");
        PrintLine(" restart             restart level");
        PrintLine(" help                list all available commands");
        PrintLine(" ls                  list all files");
        PrintLine(" cat [FILENAME]      open file specified by [FILENAME]");
        PrintLine(" cd ..               return to main menu");
        PrintLine(" quit                quit game");
        
        PrintLine("");
            
        PrintLine("Click anywhere to start hacking...");
    }
    
    private void HideCreditMenu()
    {
        ClearScrollView();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
    }
    
    protected override void ProcessCommand(string cmd)
    {
        string command = string.Empty;
        string fileName = string.Empty;
        if (cmd.Length > 2 && cmd.Contains("cat"))
        {
            command = cmd.Substring(0, 3);
            fileName = cmd.Substring(3).Trim();
        }
        string detectCmd = command == string.Empty ? cmd : command;
        switch (detectCmd.ToLower())
        {
            case "help":
                PrintLine("Available commands:");
                PrintColumns(" restart", "restart level");
                PrintColumns(" help", "list all available commands");
                PrintColumns(" ls", "list all files");
                PrintColumns(" cat [FILENAME]", "open file specified by [FILENAME]");
                
                PrintColumns(" cd ..", "return to main menu");
                PrintColumns(" quit", "quit game");
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
            case "ls":
                PrintLine("Printing all the available documents:");
                PrintLine("");
                print("current level: " + GameManager.Instance.currentLevel);
                foreach (FileDetails document in DownloadsManager.Instance.DownloadedFiles
                             .Where(file => file.levelOfDownload == GameManager.Instance.currentLevel))
                {
                    PrintLine(document.FileName);
                }
                break;
            case "cat":
                PrintLine("Open the specified document: " + fileName);
                foreach (FileDetails document in DownloadsManager.Instance.DownloadedFiles
                             .Where(file => file.levelOfDownload == GameManager.Instance.currentLevel))
                {
                    if (document.FileName == fileName)
                    {
                        Image image = new Image();
                        image.sprite = document.documentSprite;
                        image.AddToClassList("image");
                        _output.Add(image);
                    }
                }
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
}
