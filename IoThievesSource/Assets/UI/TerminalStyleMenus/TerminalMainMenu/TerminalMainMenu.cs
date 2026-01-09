using System;
using Events;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalMainMenu : TerminalMenu
{
    
    private Label _loadingLabel;
    private float _loadingProgress = 0f;
    private int _loadingBarLength = 20;
    private float _loadingSpeed = 0.1f;

    protected override void OnMenuEnabled()
    {
        StartScanning(IothievesScan);
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
        if (_hasStartedScanning && _scannerTimer >= _scanCharTypingDelay)
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
                PrintLine($"iothieves@kali:~$ {IothievesScan}");
                PrintLine("");
                PrintLine("IoThieves v0.1.2");
                PrintLine("Copyright (c) 2025 by IoTeam");
                PrintLine("Polimi Game Collective");
                
                StartLoading();
                
                _scannerInput = string.Empty;
                
                _currentInput = string.Empty;
                _caretIndex = 0;
                _inputLabel.text = string.Empty;
                UpdateCaretPosition();
            }
        }
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

        if (_loadingProgress >= 1f)
        {
            _isLoading = false;
            PrintLine("");

            _loadingLabel.text = "[####################] 100%";
            
            PrintLine("");
            
            PrintLine("[OK] Scan completed.");
            PrintLine("42 vulnerable devices found.");
            PrintLine("12 XSS vulnerabilities found.");
            PrintLine("20 SQL injection vulnerabilities found.");
            PrintLine("10 Binary exploitation vulnerabilities found.");
            PrintLine("42 devices exploited.");
            
            PrintLine("");
            
            PrintLine("Type 'iothieves --help' to list commands");
        }
    }

    private void UpdateLoadingBarText()
    {
        int loaded = Mathf.RoundToInt(_loadingProgress * _loadingBarLength);
        int empty = _loadingBarLength - loaded;

        string bar = "[" + new string('#', loaded) + new string(' ', empty) + $"] {(int)(_loadingProgress * 100)}%";
        _loadingLabel.text = bar;
    }

    private void InsertCharacter(char character)
    {
        _currentInput = _currentInput.Insert(_caretIndex, character.ToString());
        _caretIndex++;
        
        _inputLabel.text = _currentInput;
        UpdateCaretPosition();
    }
    
    protected override void ProcessCommand(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "iothieves --help":
                PrintLine("Available commands: iothieves --start, iothieves --options, iothieves --exit");
                break;
            case "iothieves --start":
                PrintLine("Connection established...");
                EventsManager.OnEnterLevelSelection?.Invoke();
                gameObject.SetActive(false);
                break;
            case "iothieves --options":
                PrintLine("Customization session started...");
                EventsManager.OnOptionsClicked?.Invoke();
                break;
            case "iothieves --exit":
                PrintLine("Shutting down...");
                EventsManager.OnQuitGame?.Invoke();
                break;
            default:
                PrintLine($"bash: {cmd}: command not found");
                break;
        }
    }
}
