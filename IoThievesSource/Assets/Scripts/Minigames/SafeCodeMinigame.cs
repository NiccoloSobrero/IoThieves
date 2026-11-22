using System.Collections;
using System.Text;
using Events;
using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames
{
    public class SafeCodeMinigame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private GameObject solutionObj;
        [SerializeField] private float waitTime;

        private bool _foundSolution;

        private bool _isCompleting;

        private int _passwordLength;
        private string _targetPassword;
        private int _typedPasswordLength;
        private StringBuilder _currentTypedPassword;

        #region Events

        [HideInInspector] public UnityEvent onGuessedPassword;

        #endregion

        private void Awake()
        {
            _foundSolution = false;
        }

        private void OnEnable()
        {
            EventsManager.OnFoundCodePadSolution.AddListener(MarkSolutionFound);
        }

        private void OnDisable()
        {
            EventsManager.OnFoundCodePadSolution.RemoveListener(MarkSolutionFound);
        }

        private void MarkSolutionFound()
        {
            _foundSolution = true;
        }

        public void InitMinigame(string targetPassword)
        {
            _targetPassword = targetPassword;
            _passwordLength = targetPassword.Length;
            _currentTypedPassword = new StringBuilder();
            _typedPasswordLength = 0;
            _isCompleting = false;

            for (int i = 0; i < _passwordLength; i++)
            {
                _currentTypedPassword.Append('_');
            }
            
            displayText.text = _currentTypedPassword.ToString();

            solutionObj.SetActive(_foundSolution);
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
            _currentTypedPassword.Clear();
            
            for (int i = 0; i < _passwordLength; i++)
            {
                _currentTypedPassword.Append('_');
            }
            
            displayText.text = _currentTypedPassword.ToString();

            _typedPasswordLength = 0;
        }

        public void Confirm()
        {
            if (_isCompleting) return;

            if (_currentTypedPassword.Equals(_targetPassword))
            {
                _isCompleting = true;
                StartCoroutine(CompleteMinigameAfter(waitTime));
            }
        }

        private IEnumerator CompleteMinigameAfter(float time)
        {
            yield return new WaitForSeconds(time);
            
            EventsManager.OnMinigameCompleted?.Invoke();
            
            onGuessedPassword?.Invoke();
        }
    }
}