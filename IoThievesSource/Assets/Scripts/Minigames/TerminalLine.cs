using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

namespace Minigames
{
    public class TerminalLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI addressText;
    
        [SerializeField] private TextMeshProUGUI lineLeftText;
        [SerializeField] private TextMeshProUGUI lineButtonText;
        [SerializeField] private TextMeshProUGUI lineRightText;
    
        [SerializeField] private RectTransform lineLeftRect;
        [SerializeField] private RectTransform lineButtonRect;
        [SerializeField] private RectTransform lineRightRect;
    
        private readonly int _lineLen = 30;
        private readonly int _addrLen = 4;
        private readonly int _buttonLineLen = 5;

        private int _leftLineLen;
        private int _rightLineLen;
    
        private readonly List<char> _specialChars = new(){'&', '!', '?', '$', '%', '=', '[', ']', '\"', '*', '{', '}', 
            ':', ';', '-', '+', '@', '#', '(', ')', '\\', '^', '|', '\'', '~', '<', '>', ',', '.'};

        private readonly List<char> _addressChars = new() {'C', 'F', 'B', 'A', 'D', 'E', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private string _word;

        public void Customize(string word)
        {
            _word = word;
        
            _leftLineLen = Random.Range(0, _lineLen - _buttonLineLen);
        
            string newAddr = "0x";
            for (int i = 0; i < _addrLen; i++)
            {
                newAddr += _addressChars[Random.Range(0, _addressChars.Count)];
            }
            addressText.text = newAddr;
             
            // Set line left text
            string newLineLeft = "";
            for (int i = 0; i < _leftLineLen; i++)
            {
                newLineLeft += _specialChars[Random.Range(0, _specialChars.Count)];
            }
            lineLeftText.text = newLineLeft;

            // Set line button text
            lineButtonText.text = word;
        
            // Set line right text
            newLineLeft = "";
            _rightLineLen = _lineLen - _buttonLineLen - _leftLineLen;
            for (int i = 0; i < _rightLineLen; i++)
            {
                newLineLeft += _specialChars[Random.Range(0, _specialChars.Count)];
            }
            lineRightText.text = newLineLeft;
        
            // Set dimensions
            lineLeftRect.sizeDelta = new Vector2(lineLeftText.preferredWidth, lineLeftRect.sizeDelta.y);
            lineButtonRect.sizeDelta = new Vector2(lineButtonText.preferredWidth, lineButtonRect.sizeDelta.y);
            lineRightRect.sizeDelta = new Vector2(lineRightText.preferredWidth, lineRightRect.sizeDelta.y);
        }

        public void OnClick()
        {
            EventsManager.OnClickDecryptPassword?.Invoke(_word);
        }
    }
}