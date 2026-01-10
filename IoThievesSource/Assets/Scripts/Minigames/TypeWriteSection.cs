using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minigames
{
    [RequireComponent(typeof(ButtonSelectionListener), typeof(Button))]
    public class TypeWriteSection : MonoBehaviour
    {
        private TypeWriteMinigame _minigame;
        
        [Header("UI")]
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private Image caretHighlight;
        private readonly Vector2 _caretPadding = new(2f, 2f);

        [Header("Text")]
        [TextArea(4, 12)]
        [SerializeField] private string textToType =
            "The quick brown fox jumps over the lazy dog.\n\n" +
            "Practice accuracy first, then speed.";

        [Header("Colors")]
        private readonly Color32 _baseGrey = new(130, 130, 130, 255);
        private readonly Color32 _correct = new(120, 210, 140, 255);
        private readonly Color32 _wrong = new(235, 120, 120, 255);
        
        private readonly Color32 _caretBg = new(140, 140, 140, 120);

        [Header("Behavior")]
        [SerializeField] private bool allowMistakes = true;
        [SerializeField] private bool allowBackspace = true;
        
        private ButtonSelectionListener _buttonSelectionListener;
        
        private int _index;
        private char[] _typed;
        private bool[] _correctAtIndex;

        private bool _isSelected;

        private void Awake()
        {
            _buttonSelectionListener = GetComponent<ButtonSelectionListener>();
        }
        
        private void OnEnable()
        {
            _buttonSelectionListener.OnSelected += OnSelect;
            _buttonSelectionListener.OnDeselected += OnDeselect;
        }

        private void OnDisable()
        {
            _buttonSelectionListener.OnSelected -= OnSelect;
            _buttonSelectionListener.OnDeselected -= OnDeselect;
        }

        public void Init(TypeWriteMinigame minigame)
        {
            _minigame = minigame;
            
            textToType = textToType.Replace("\r\n", "\n").Replace("\r", "\n");

            caretHighlight.color = _caretBg;
            caretHighlight.gameObject.SetActive(false);
            
            _index = 0;
            _isSelected = false;

            _typed = new char[textToType.Length];
            _correctAtIndex = new bool[textToType.Length];

            Render();
        }

        private void Update()
        {
            if (!_isSelected) return;
            
            string input = Input.inputString;
            if (string.IsNullOrEmpty(input))
                return;

            foreach (char c in input)
            {
                if (c == '\b')
                {
                    HandleBackspace();
                }
                else if (c == '\n' || c == '\r')
                {
                    HandleChar('\n');
                }
                else
                {
                    HandleChar(c);
                }
            }
        }

        private void HandleChar(char typedChar)
        {
            if (_index >= textToType.Length)
                return;

            char expected = textToType[_index];
            if (expected == '\r') expected = '\n';

            bool isCorrect = typedChar == expected;

            if (!allowMistakes && !isCorrect)
                return;

            _typed[_index] = typedChar;
            _correctAtIndex[_index] = isCorrect;

            _index++;
            Render();

            if (_index >= textToType.Length && !HasTooManyErrors())
            {
                // Text was completed
                StartCoroutine(_minigame.NextSection());
            }
        }

        private void HandleBackspace()
        {
            if (!allowBackspace || _index <= 0)
                return;

            _index--;
            Render();
        }
        
        private void Render()
        {
            var sb = new StringBuilder(textToType.Length * 8);

            // typed part
            for (int i = 0; i < _index; i++)
            {
                char ch = textToType[i];
                if (ch == '\r') continue;

                sb.Append(ColorTag(_correctAtIndex[i] ? _correct : _wrong));
                sb.Append(Escape(ch));
                sb.Append("</color>");
            }

            // remaining text
            for (int i = _index; i < textToType.Length; i++)
            {
                char ch = textToType[i];
                if (ch == '\r') continue;

                sb.Append(ColorTag(_baseGrey));
                sb.Append(Escape(ch));
                sb.Append("</color>");
            }

            targetText.text = sb.ToString();
            targetText.ForceMeshUpdate(); // ensure textInfo is up-to-date
            UpdateCaretHighlight();
        }
        
        private void UpdateCaretHighlight()
        {
            if (caretHighlight == null || targetText == null)
                return;

            if (_index < 0 || _index >= textToType.Length)
            {
                caretHighlight.enabled = false;
                return;
            }

            // Make sure TMP generated geometry
            var ti = targetText.textInfo;
            if (ti == null || ti.characterCount == 0 || _index >= ti.characterCount)
            {
                caretHighlight.enabled = false;
                return;
            }

            var ch = ti.characterInfo[_index];
            int line = ch.lineNumber;

            // Even if a space isn't "visible", it still has layout metrics (origin/xAdvance).
            float left = ch.origin;
            float right = ch.xAdvance;

            var lineInfo = ti.lineInfo[line];
            float top = lineInfo.ascender;
            float bottom = lineInfo.descender;

            float width = Mathf.Max(1f, right - left);
            float height = Mathf.Max(1f, top - bottom);

            // Center of the caret box in TMP local space
            Vector2 center = new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f);

            var rt = caretHighlight.rectTransform;
            caretHighlight.enabled = true;
            
            rt.anchoredPosition = center;
            rt.sizeDelta = new Vector2(width, height) + _caretPadding;
        }

        private string ColorTag(Color32 c)
            => $"<color=#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}>"; // :X2 is the format where "X" means hexadecimal and "2" always shows 2 digits

        private string Escape(char ch)
        {
            return ch switch
            {
                '<' => "&lt;",
                '>' => "&gt;",
                '&' => "&amp;",
                '\n' => "\n",
                '\t' => "    ",
                _ => ch.ToString()
            };
        }

        public void OnSelect(BaseEventData _)
        {
            _isSelected = true;
            caretHighlight.gameObject.SetActive(true);
        }

        public void OnDeselect(BaseEventData _)
        {
            _isSelected = false;
            caretHighlight.gameObject.SetActive(false);
        }

        public bool HasTooManyErrors()
        {
            int errorCount = 0;
            
            for (int i = 0; i < _correctAtIndex.Length; i++)
            {
                if (!_correctAtIndex[i])
                {
                    errorCount++;
                }
            }
            
            return errorCount > _correctAtIndex.Length * 0.1f;
        }
    }
}
