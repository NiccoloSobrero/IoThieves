using UnityEngine;
using UnityEngine.UIElements;

public abstract class TerminalMenu : MonoBehaviour
{
    [SerializeField]
    protected UIDocument uiDocument;
    [SerializeField]
    protected AudioSource _typingAudioSource;
    [SerializeField]
    protected AudioClip[] _typingAudioClips;
    [SerializeField]
    protected AudioClip _backspaceClip;
    [SerializeField]
    protected AudioClip _enterClip;

    protected ScrollView _output;
    protected VisualElement _caret;
    protected VisualElement _inputContainer;
    protected Label _inputLabel;

    protected string _currentInput = "";
    protected int _caretIndex = 0;

    protected bool _caretVisible = true;
    protected float _blinkTimer = 0f;
    
    protected bool _hasStartedScanning = false;
    protected float _scannerTimer = 0f;
    protected float[] _scanCharTypingDelay = { 0.1f, 1f };
    protected int _scanIndex = 0;
    protected string _scannerInput = null;
    
    protected bool _isLoading = false;
    
    protected const string IothievesScan = "iothieves --scan 192.168.200.0/24";
    
    
    protected void OnEnable()
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
        
        OnMenuEnabled();
    }
    
    protected virtual void OnMenuEnabled() { }

    protected void Update()
    {
        _blinkTimer += Time.unscaledDeltaTime;
        if (_blinkTimer >= 0.5f)
        {
            _blinkTimer = 0f;
            _caretVisible = !_caretVisible;
            _caret.style.opacity = _caretVisible ? 1f : 0f;
        }
        
        _scannerTimer += Time.unscaledDeltaTime;

        OnUpdate();
    }
    
    protected virtual void OnUpdate() { }
    
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
            _typingAudioSource.PlayOneShot(_backspaceClip, 0.5f);
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
            PlaySFX();
        }
        else if (evt.keyCode == KeyCode.RightArrow)
        {
            _caretIndex = Mathf.Min(_currentInput.Length, _caretIndex + 1);
            PlaySFX();
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
            _typingAudioSource.PlayOneShot(_enterClip, 0.5f);
        }
        else if (!char.IsControl(evt.character))
        {
            // _currentInput = _currentInput.Insert(_caretIndex, evt.character.ToString());
            // _caretIndex++;
            InsertCharacter(evt.character);
            textChanged = true;
        }

        if (textChanged)
        {
            _inputLabel.text = _currentInput;
        }

        UpdateCaretPosition();
        evt.StopPropagation();
    }
    
    protected void InsertCharacter(char character)
    {
        _currentInput = _currentInput.Insert(_caretIndex, character.ToString());
        _caretIndex++;
        
        _inputLabel.text = _currentInput;
        UpdateCaretPosition();

        PlaySFX();
    }
    
    protected void UpdateCaretPosition()
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
    
    protected void PrintLine(string text)
    {
        var label = new Label(text);
        label.AddToClassList("prompt-header");
        _output.Add(label);
        _output.ScrollTo(label);
    }
    
    protected void PrintBanner(string banner)
    {
        Label label = new Label(banner);
        label.AddToClassList("terminal-banner");
        _output.Add(label);
    }
    
    protected void PrintColumns(string left, string right)
    {
        VisualElement row = new VisualElement();
        row.AddToClassList("row");
        
        Label leftLabel = new Label(left);
        leftLabel.AddToClassList("left-col");
        Label rightLabel = new Label(right);
        rightLabel.AddToClassList("right-col");
        
        row.Add(leftLabel);
        row.Add(rightLabel);
        
        _output.Add(row);
        _output.ScrollTo(row);
    }

    protected void PlaySFX()
    {
        if (_typingAudioSource == null || _typingAudioClips == null || _typingAudioClips.Length == 0)
        {
            return;
        }
        
        AudioClip clip = _typingAudioClips[Random.Range(0, _typingAudioClips.Length)];
        _typingAudioSource.PlayOneShot(clip, 0.6f);
    }

    protected abstract void ProcessCommand(string cmd);
}
