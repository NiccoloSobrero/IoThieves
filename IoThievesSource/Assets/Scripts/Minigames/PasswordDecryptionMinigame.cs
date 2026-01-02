using System.Collections;
using System.Collections.Generic;
using Events;
using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    public enum LetterResult
    {
        Absent,
        Present,
        Correct
    }
    
    public class PasswordDecryptionMinigame : MonoBehaviour
    {
        [SerializeField] private List<TerminalLine> terminalLines = new();
        [SerializeField] private List<GameObject> heartsObjects = new();
        
        [SerializeField] private GameObject decryptionOutputObject;
        [SerializeField] private Transform decryptionOutputParent;
        
        private string _solution;

        private readonly List<string> _possibleWords = new() { 
            "HACKS",
            "STEAL",
            "CODEX",
            "CRYPT",
            "CLOUD",
            "LOGIN",
            "TOKEN",
            "PROXY",
            "PATCH",
            "AGENT",
            "SERVO",
            "ROBOT",
            "DRIVE",
            "ALERT",
            "TRACE",
            "SCANS",
            "BYTES",
            "NODES",
            "LOGIC",
            "SNIFF",
            "RADIO",
            "SENSE",
            "INPUT",
            "DELTA",
            "SWIPE",
            "ALPHA",
            "SMART",
            "ROUTE",
            "CACHE",
            "PINGS"
        };

        private int _currentLeftAttempts;
        
        private readonly string _colorCloseStr = "</color>";
        private readonly string _correctColor = "<color=#00FF00>";
        private readonly string _presentColor = "<color=#FFFF00>";
        private readonly string _absentColor = "<color=#FF0000>";

        private float _bootingTime = 3f;

        private bool _canCheckGuess;
        
        private void OnEnable()
        {
            EventsManager.OnClickDecryptPassword.AddListener(CheckSolution);
        }

        private void OnDisable()
        {
            EventsManager.OnClickDecryptPassword.RemoveListener(CheckSolution);
        }

        public void InitMinigame()
        {
            // Init hearts
            _currentLeftAttempts = heartsObjects.Count;

            foreach (GameObject heart in heartsObjects)
            {
                heart.SetActive(true);
            }
            
            // Init decryption outputs
            foreach (Transform child in decryptionOutputParent)
            {
                child.gameObject.SetActive(false);
            }
            
            // Init terminal lines graphics and logic
            List<string> wordsClone = new List<string>(_possibleWords);
        
            _solution = wordsClone[Random.Range(0, wordsClone.Count)];
            wordsClone.Remove(_solution);
            
            int solutionIndex = Random.Range(0, terminalLines.Count);
            int currIndex = 0;
            
            foreach (TerminalLine line in terminalLines)
            {
                if (currIndex == solutionIndex)
                {
                    line.Customize(_solution);
                }
                else
                {
                    string selectedWord = wordsClone[Random.Range(0, wordsClone.Count)];
                
                    line.Customize(selectedWord);
                    wordsClone.Remove(selectedWord);
                }

                currIndex++;
            }

            _canCheckGuess = true;
        }

        private void CheckSolution(string guess)
        {
            if (!_canCheckGuess)
            {
                return;
            }
            
            // Print on screen the result -> pass or fail (How many correct letters)
            if (guess != _solution)
            {
                PrintOutputOnWrongGuess(guess);
                
                heartsObjects[_currentLeftAttempts - 1].SetActive(false);

                _currentLeftAttempts--;

                if (_currentLeftAttempts <= 0)
                {
                    Debug.Log("GAME OVER. Restarting Game.");
                    
                    InitMinigame();
                }
            }
            else
            {
                PrintOutputOnCorrectGuess(guess);
                
                _canCheckGuess = false;

                StartCoroutine(CompleteMinigameAfter(_bootingTime));
            }
        }

        private IEnumerator CompleteMinigameAfter(float time)
        {
            yield return new WaitForSeconds(time);
            
            EventsManager.OnMinigameCompleted?.Invoke();
        }

        private LetterResult[] GetCorrectnessArray(string guess)
        {
            int length = _solution.Length;
            var result = new LetterResult[length];
            
            // Check correctness of words
            var remainingCounts = new Dictionary<char, int>();

            for (int i = 0; i < length; i++)
            {
                char s = _solution[i];
                char g = guess[i];

                if (g == s)
                {
                    result[i] = LetterResult.Correct;
                }
                else
                {
                    remainingCounts.TryAdd(s, 0);
                    remainingCounts[s]++;
                }
            }

            // Check for non-correct letters
            for (int i = 0; i < length; i++)
            {
                if (result[i] == LetterResult.Correct)
                {
                    continue;
                }

                char g = guess[i];

                if (remainingCounts.TryGetValue(g, out int count) && count > 0)
                {
                    result[i] = LetterResult.Present;
                    remainingCounts[g] = count - 1;
                }
                else
                {
                    result[i] = LetterResult.Absent;
                }
            }

            return result;
        }
        
        private void PrintOutputOnCorrectGuess(string guess)
        {
            TextMeshProUGUI text1 = Instantiate(decryptionOutputObject, decryptionOutputParent).GetComponent<TextMeshProUGUI>();
            
            TextMeshProUGUI text2 = Instantiate(decryptionOutputObject, decryptionOutputParent).GetComponent<TextMeshProUGUI>();
            
            TextMeshProUGUI text3 = Instantiate(decryptionOutputObject, decryptionOutputParent).GetComponent<TextMeshProUGUI>();
            
            text1.text = "> " + _correctColor + guess + _colorCloseStr;
            text2.text = "> Access granted";
            // text3.text = "> Booting in ";
            StartCoroutine(UpdateWaitTimeText(text3, _bootingTime));
            
            UpdateOutputContentHeight();
        }

        private IEnumerator UpdateWaitTimeText(TextMeshProUGUI label, float time)
        {
            float timer = time;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                label.text = "> Booting in " + (int)(timer * 100) / 100f + "s";

                yield return null;
            }
            
            label.text = "> Booting in 0s";
        }
        
        private void PrintOutputOnWrongGuess(string guess)
        {
            LetterResult[] results = GetCorrectnessArray(guess);
            
            TextMeshProUGUI text1 = Instantiate(decryptionOutputObject, decryptionOutputParent).GetComponent<TextMeshProUGUI>();
            
            TextMeshProUGUI text2 = Instantiate(decryptionOutputObject, decryptionOutputParent).GetComponent<TextMeshProUGUI>();
            
            TextMeshProUGUI text3 = Instantiate(decryptionOutputObject, decryptionOutputParent).GetComponent<TextMeshProUGUI>();
            
            string guessText = "> ";

            for (int i = 0; i < results.Length; i++)
            {
                switch (results[i])
                {
                    case LetterResult.Correct:
                        guessText += _correctColor;
                        break;
                    case LetterResult.Present:
                        guessText += _presentColor;
                        break;
                    case LetterResult.Absent:
                        guessText += _absentColor;
                        break;
                }
                
                guessText += guess[i] + _colorCloseStr;
            }
            
            text1.text = guessText;

            int guessedLetters = 0;
            foreach (LetterResult result in results)
            {
                if (result == LetterResult.Correct)
                {
                    guessedLetters++;
                }
            }
            
            text2.text = "> " + guessedLetters + " / " + guess.Length;
            
            text3.text = "> Entry denied";
            
            UpdateOutputContentHeight();
        }

        private void UpdateOutputContentHeight()
        {
            GridLayoutGroup grid = decryptionOutputParent.GetComponent<GridLayoutGroup>();
            
            float totalHeight = 0;
            
            float singleElementHeight = grid.cellSize.y;
            
            float paddingTop = grid.padding.top;
            float paddingBottom = grid.padding.bottom;
            
            float ySpacing = grid.spacing.y;
            
            foreach (Transform child in decryptionOutputParent)
            {
                if (child.gameObject.activeSelf)
                {
                    totalHeight += singleElementHeight + ySpacing;
                }
            }

            totalHeight += paddingBottom + paddingTop - ySpacing;
            
            RectTransform outputRect = decryptionOutputParent.GetComponent<RectTransform>();
            
            outputRect.sizeDelta = new Vector2(0, totalHeight);

            float yPos = Mathf.Clamp(outputRect.sizeDelta.y - 650, 0, outputRect.sizeDelta.y - 650);
            outputRect.anchoredPosition = new Vector2(0, yPos);
        }
    }
}