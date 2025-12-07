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

        private int _passwordLength;
        private string _targetPassword;
        private int _typedPasswordLength;
        private StringBuilder _currentTypedPassword;

        private LevelObjectiveScrObj _objective;

        #region Events

        [HideInInspector] public UnityEvent onGuessedPassword;

        #endregion

        private void Awake()
        {
            _foundSolution = false;
        }

        private void OnEnable()
        {
            EventsManager.OnFoundCodePadSolution.AddListener(() => _foundSolution = true);
        }

        private void OnDisable()
        {
            EventsManager.OnFoundCodePadSolution.RemoveListener(() => _foundSolution = true);
        }

        public void InitMinigame(string targetPassword, LevelObjectiveScrObj objective)
        {
            _objective = objective;
            
            _targetPassword = targetPassword;
            _passwordLength = targetPassword.Length;
            _currentTypedPassword = new StringBuilder();
            _typedPasswordLength = 0;

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
            if (_currentTypedPassword.Equals(_targetPassword))
            {
                StartCoroutine(CompleteMinigameAfter(waitTime));
            }
        }

        private IEnumerator CompleteMinigameAfter(float time)
        {
            yield return new WaitForSeconds(time);
            
            EventsManager.OnMinigameCompleted?.Invoke();
            
            _objective?.Complete();
            
            onGuessedPassword?.Invoke();
        }
    }
}
