using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalMainMenu : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    private ScrollView _output;
    private VisualElement _caret;
    private VisualElement _inputContainer;
    private Label _inputLabel;

    private string _currentInput = "";
    private int _caretIndex = 0;

    private bool _caretVisible = true;
    private float _blinkTimer = 0f;
    
    private bool _hasStartedScanning = false;
    private float _scannerTimer = 0f;
    private float _scanCharTypingDelay = 0.1f;
    private int _scanIndex = 0;
    private string _scannerInput = null;
    
    private const string iothievesScan = "iothieves --scan 192.168.200.0/24";
    
    private Label _loadingLabel;
    private float _loadingProgress = 0f;
    private bool _isLoading = false;
    private int _loadingBarLength = 20;
    private float _loadingSpeed = 0.1f;
    

    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        _output = root.Q<ScrollView>("terminalOutput");
        
        _inputContainer = root.Q<VisualElement>("inputContainer");
        _inputLabel = _inputContainer.Q<Label>("inputText");

        _caret = new VisualElement();
        _caret.AddToClassList("caret");
        _inputContainer.Add(_caret);
        
        root.focusable = true;
        root.tabIndex = 0;
        root.Focus();

        root.RegisterCallback<PointerDownEvent>(_ => root.Focus());
        
        root.RegisterCallback<KeyDownEvent>(OnKeyDown);
        
        _currentInput = string.Empty;
        _caretIndex = 0;
        _inputLabel.text = string.Empty;

        UpdateCaretPosition();

        // PrintLine("IoThieves v0.1.2");
        // PrintLine("Copyright (c) 2025 by IoTeam");
        // PrintLine("Polimi Game Collective");
        //
        // PrintLine("Type 'help' to list commands");
        StartScanning(iothievesScan);
    }

    void Update()
    {
        _blinkTimer += Time.unscaledDeltaTime;
        if (_blinkTimer >= 0.5f)
        {
            _blinkTimer = 0f;
            _caretVisible = !_caretVisible;
            _caret.style.opacity = _caretVisible ? 1f : 0f;
        }
        
        _scannerTimer += Time.unscaledDeltaTime;
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
                PrintLine($"iothieves@kali:~$ {iothievesScan}");
                
                StartLoading();
                
                _scannerInput = string.Empty;
                
                _currentInput = string.Empty;
                _caretIndex = 0;
                _inputLabel.text = string.Empty;
                UpdateCaretPosition();
                
                // PrintLine("IoThieves v0.1.2");
                // PrintLine("Copyright (c) 2025 by IoTeam");
                // PrintLine("Polimi Game Collective");
                //
                // PrintLine("Type 'help' to list commands");
            }
        }

        if (!_isLoading) return;
        
        print("loading");
        
        _loadingProgress += Time.unscaledDeltaTime * _loadingSpeed;
        _loadingProgress = Mathf.Clamp01(_loadingProgress);

        UpdateLoadingBarText();

        if (_loadingProgress >= 1f)
        {
            _isLoading = false;
            PrintLine("IoThieves v0.1.2");
            PrintLine("Copyright (c) 2025 by IoTeam");
            PrintLine("Polimi Game Collective");
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
            
            PrintLine("Type 'help' to list commands");
        }
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

    private void UpdateLoadingBarText()
    {
        print($"_loadingProgress {_loadingProgress}");
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

    private void OnKeyDown(KeyDownEvent evt)
    {
        if (_hasStartedScanning || _isLoading)
        {
            evt.StopPropagation();
            return;
        }
        bool textChanged = false;

        if (evt.keyCode == KeyCode.Backspace)
        {
            if (_caretIndex > 0)
            {
                _currentInput = _currentInput.Remove(_caretIndex - 1, 1);
                _caretIndex--;
                textChanged = true;
            }
        }
        else if (evt.keyCode == KeyCode.LeftArrow)
        {
            _caretIndex = Mathf.Max(0, _caretIndex - 1);
        }
        else if (evt.keyCode == KeyCode.RightArrow)
        {
            _caretIndex = Mathf.Min(_currentInput.Length, _caretIndex + 1);
        }
        else if (evt.keyCode == KeyCode.Return)
        {
            if (!string.IsNullOrWhiteSpace(_currentInput))
            {
                PrintLine($"iothieves@kali:~$ {_currentInput}");
                ProcessCommand(_currentInput);
            }

            _currentInput = string.Empty;
            _caretIndex = 0;
            textChanged = true;
        }
        else if (!char.IsControl(evt.character))
        {
            _currentInput = _currentInput.Insert(_caretIndex, evt.character.ToString());
            _caretIndex++;
            textChanged = true;
        }

        if (textChanged)
        {
            _inputLabel.text = _currentInput;
        }

        UpdateCaretPosition();
        evt.StopPropagation();
    }

    private void UpdateCaretPosition()
    {
        string beforeCaret =
            _caretIndex > 0
                ? _currentInput.Substring(0, _caretIndex)
                : string.Empty;

        Vector2 textSize = _inputLabel.MeasureTextSize(
            beforeCaret,
            0,
            VisualElement.MeasureMode.Undefined,
            0,
            VisualElement.MeasureMode.Undefined
        );

        _caret.style.left = textSize.x;
        _caret.style.top = 0;
    }

    private void PrintLine(string text)
    {
        var label = new Label(text);
        label.AddToClassList("prompt-header");
        _output.Add(label);
        _output.ScrollTo(label);
    }

    private void ProcessCommand(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "help":
                PrintLine("Available commands: start, exit");
                break;
            case "start":
                PrintLine("Starting hack sequence...");
                break;
            case "exit":
                PrintLine("Shutting down...");
                Application.Quit();
                break;
            default:
                PrintLine($"Command not found: {cmd}");
                break;
        }
    }
}
