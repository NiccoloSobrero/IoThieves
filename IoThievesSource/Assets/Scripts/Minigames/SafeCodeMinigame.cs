using System.Collections;
using System.Text;
using Events;
using TMPro;
using UnityEngine;

namespace Minigames
{
    public class SafeCodeMinigame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private float waitTime;

        private int _passwordLength;
        private string _targetPassword;
        private int _typedPasswordLength;
        private StringBuilder _currentTypedPassword;
        
        public void InitMinigame(string targetPassword)
        {
            _targetPassword = targetPassword;
            _passwordLength = targetPassword.Length;
            _currentTypedPassword = new StringBuilder();
            _typedPasswordLength = 0;

            for (int i = 0; i < _passwordLength; i++)
            {
                _currentTypedPassword.Append('_');
            }
            
            displayText.text = _currentTypedPassword.ToString();
        }
    
        public void OnDigitClicked(string digit)
        {
            if (_typedPasswordLength >= _passwordLength) return;
            
            _currentTypedPassword[_typedPasswordLength] = digit[0];
            displayText.text = _currentTypedPassword.ToString();
            
            _typedPasswordLength++;
        }
    
        public void ClearText()
        {
            for (int i = 0; i < _passwordLength; i++)
            {
                _currentTypedPassword.Append('_');
            }
            
            displayText.text = _currentTypedPassword.ToString();

            _typedPasswordLength = 0;
        }

        public void Confirm()
        {
            if (_currentTypedPassword.Equals(_targetPassword))
            {
                Debug.Log($"{displayText.text} is the correct password.");
                
                StartCoroutine(WaitForCompletion(waitTime));
            }
            else
            {
                Debug.Log($"{displayText.text} is not the correct password.");
            }
        }

        private IEnumerator WaitForCompletion(float time)
        {
            yield return new WaitForSeconds(time);
            
            EventsManager.OnMinigameCompleted?.Invoke();
        }
    }
}
