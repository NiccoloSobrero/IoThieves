using System;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TerminalMainMenu : TerminalMenu
{
    
    private Label _loadingLabel;
    private float _loadingProgress = 0f;
    private int _loadingBarLength = 20;
    private float _loadingSpeed = 0.3f;

    protected override void OnMenuEnabled()
    {
        if (GameManager.Instance.ReloadMainMenuAnimation)
        {
            StartScanning(IothievesScan);
        }
        else
        {
            PrintStartMainMenu();
            PrintEndMainMenu();
        }
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.SetContext(InputContext.LevelSelection);
    }

    protected override void OnUpdate()
    {
        Scan();
        Load();
    }

    private void StartScanning(string text)
    {
        _scannerInput = text;
        _scanIndex = 0;
        _scannerTimer = 0f;
        _hasStartedScanning = true;
        
        _currentInput = string.Empty;
        _caretIndex = 0;
        _inputLabel.text = string.Empty;
        UpdateCaretPosition();
    }

    private void Scan()
    {
        float delay = Random.Range(_scanCharTypingDelay[0], _scanCharTypingDelay[1]);
        if (_hasStartedScanning && _scannerTimer >= delay)
        {
            _scannerTimer = 0f;

            if (_scanIndex < _scannerInput.Length)
            {
                InsertCharacter(_scannerInput[_scanIndex]);
                _scanIndex++;
            }
            else
            {
                _hasStartedScanning = false;

                PrintStartMainMenu();
                StartLoading();
                
                _scannerInput = string.Empty;
                
                _currentInput = string.Empty;
                _caretIndex = 0;
                _inputLabel.text = string.Empty;
                UpdateCaretPosition();
            }
        }
    }

    private void PrintStartMainMenu()
    {
        PrintLine($"iothieves@kali:~$ {IothievesScan}");
        // PrintLine("");
        // PrintLine("IoThieves v0.1.2");
        PrintBanner(Utility.IoThievesArt);
        PrintLine("Copyright (c) 2025");
        PrintColumns("Team Leader:", "Niccoló Sobrero");
        PrintColumns("Designer:", "Matilde Sobberi");
        PrintColumns("Architect:", "Daniel Bonardi");
        PrintColumns("AI:", "Federico Tabellini");
        PrintColumns("UI:", "Emanuele Sapio");
                
        // PrintLine("Polimi Game Collective");
        PrintLine("Developed for the Videogame Design and Programming course of the Politecnico di Milano \ntaught by Prof. Pierluca Lanzi & Prof. Daniele Loiacono.");
    }

    private void StartLoading()
    {
        _loadingProgress = 0f;
        _isLoading  = true;
        _loadingLabel = new Label();
        _loadingLabel.style.color = new Color(0.2f, 1f, 0.2f);
        _loadingLabel.style.fontSize = 24;
        _loadingLabel.style.unityFontDefinition = _inputLabel.style.unityFontDefinition;
        
        _output.Add(_loadingLabel);
        _output.ScrollTo(_loadingLabel);
    }

    private void Load()
    {
        if (!_isLoading) return;
        
        _loadingProgress += Time.unscaledDeltaTime * _loadingSpeed;
        _loadingProgress = Mathf.Clamp01(_loadingProgress);

        UpdateLoadingBarText();

        if (_loadingProgress >= 0.7f)
        {
            _isLoading = false;
            PrintLine("");

            _loadingLabel.text = "[####################] 100%";
            PrintEndMainMenu();
        }
    }

    private void PrintEndMainMenu()
    {
        PrintLine("");
            
        PrintLine("[OK] Scan completed.");
        /*PrintLine("42 vulnerable devices found.");
        PrintLine(" - 12 XSS vulnerabilities found.");
        PrintLine(" - 20 SQL injection vulnerabilities found.");
        PrintLine(" - 10 Binary exploitation vulnerabilities found.");
        PrintLine("42 devices exploited.");*/
            
        PrintLine("");
            
        // PrintLine("Type 'help' to list commands");
        PrintLine("Available commands:");
        PrintLine(" start       start game");
        PrintLine(" help        list all available commands"); //potrebbe non servire
        // PrintLine(" options     open settings");
        PrintLine(" quit        quit game");
            
        PrintLine("");
            
        PrintLine("Click anywhere and start typing the commands above..."); //"Click anywhere to start hacking..."
    }

    private void UpdateLoadingBarText()
    {
        int loaded = Mathf.RoundToInt(_loadingProgress * _loadingBarLength);
        int empty = _loadingBarLength - loaded;

        string bar = "[" + new string('#', loaded) + new string(' ', empty) + $"] {(int)(_loadingProgress * 100)}%";
        _loadingLabel.text = bar;
    }
    
    protected override void ProcessCommand(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "help":
                PrintLine("Available commands:");
                PrintLine("start            start game");
                PrintLine("help             list all available commands");
                // PrintLine("options          settings [Not Available]");
                PrintLine("quit             quit game");
                break;
            case "start":
                PrintLine("Connection established...");
                EventsManager.OnEnterLevelSelection?.Invoke();
                gameObject.SetActive(false);
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
