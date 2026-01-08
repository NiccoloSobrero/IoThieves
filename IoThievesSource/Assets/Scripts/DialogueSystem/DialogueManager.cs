using System.Collections;
using Events;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        private DialogueLinesScrObj _dialogueLinesScrObj;
        
        [SerializeField] private GameObject dialogueWindow;
        [SerializeField] private TextMeshProUGUI dialogue;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private Image speakerIcon;
        [SerializeField] private GameObject textCompletedObj;

        private const float CharactersDelay = 0.01f;
        private const float SkipTextGenerationDelay = 0f;

        private const string SpeakerPrefix = "C:\\Users\\";
        
        private int _currentLine;
        private bool _completedTextGeneration;
        private bool _canSkipTextGeneration;

        private bool _triggerOnContinueDialogue;
        
        private Coroutine _textGenerationCoroutine;
        private Coroutine _skipTextGenerationCoroutine;

        private InputContext _closingInputContext;

        #region Events

        [HideInInspector] public UnityEvent onDialogueComplete;

        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _completedTextGeneration = true;
            _canSkipTextGeneration = false;
        }

        public void SetDialogue(DialogueLinesScrObj dialogueLinesScrObj, 
            InputContext closingInputContext = InputContext.Gameplay)
        {
            _dialogueLinesScrObj = dialogueLinesScrObj;
            _closingInputContext = closingInputContext;
        }

        public void GenerateNextLine()
        {
            if (_completedTextGeneration)
            {
                if (_triggerOnContinueDialogue)
                {
                    EventsManager.OnTriggerOnContinueDialogue.Invoke();
                    _triggerOnContinueDialogue = false;
                }
                
                if (_currentLine < _dialogueLinesScrObj.dialogueLines.Length)
                {
                    dialogue.text = "> " + _dialogueLinesScrObj.dialogueLines[_currentLine].text;
                    speakerIcon.sprite = _dialogueLinesScrObj.dialogueLines[_currentLine].icon;
                    speakerName.text = SpeakerPrefix + _dialogueLinesScrObj.dialogueLines[_currentLine].speakerName.Replace(' ', '_') + ":";

                    dialogue.maxVisibleCharacters = 0;
                    textCompletedObj.SetActive(false);

                    _completedTextGeneration = false;

                    if (_dialogueLinesScrObj.dialogueLines[_currentLine].tags
                        .Contains(DialogueTag.TriggerOnContinueDialogue))
                    {
                        _triggerOnContinueDialogue = true;
                    }

                    if (_dialogueLinesScrObj.dialogueLines[_currentLine].tags.Contains(DialogueTag.TriggerOnLineStart))
                    {
                        EventsManager.OnTriggerOnLineStart.Invoke();
                    }
                    
                    TryStoppingCoroutines();

                    _textGenerationCoroutine = StartCoroutine(ShowWithDelay(CharactersDelay, dialogue.text.Length, _currentLine));
                    _skipTextGenerationCoroutine = StartCoroutine(DelaySkipTextGeneration(SkipTextGenerationDelay));

                    _currentLine++;
                }
                else
                {
                    CloseDialogueWindow();
                }
            }
            else if (_canSkipTextGeneration)
            {
                SkipTextGeneration();
            }
        }

        private IEnumerator ShowWithDelay(float letterDelay, int totalCharacters, int lineIndex)
        {
            int currentShownCharacters = 0;

            while (currentShownCharacters < totalCharacters)
            {
                currentShownCharacters++;
                dialogue.maxVisibleCharacters = currentShownCharacters;
                
                yield return new WaitForSeconds(letterDelay);
            }
            
            if (lineIndex < _dialogueLinesScrObj.dialogueLines.Length && 
                _dialogueLinesScrObj.dialogueLines[lineIndex].tags.Contains(DialogueTag.TriggerOnLineEnd))
            {
                EventsManager.OnTriggerOnLineEnd.Invoke();
            }
            
            textCompletedObj.SetActive(true);
            _completedTextGeneration = true;
        }

        private IEnumerator DelaySkipTextGeneration(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            _canSkipTextGeneration = true;
        }

        private void SkipTextGeneration()
        {
            TryStoppingCoroutines();
            
            dialogue.maxVisibleCharacters = dialogue.text.Length;
            
            if (_currentLine < _dialogueLinesScrObj.dialogueLines.Length && 
                _dialogueLinesScrObj.dialogueLines[_currentLine].tags.Contains(DialogueTag.TriggerOnLineEnd))
            {
                EventsManager.OnTriggerOnLineEnd.Invoke();
            }
            
            textCompletedObj.SetActive(true);
            _completedTextGeneration = true;
            _canSkipTextGeneration = false;
        }

        private void TryStoppingCoroutines()
        {
            if (_textGenerationCoroutine != null)
            {
                StopCoroutine(_textGenerationCoroutine);
            }

            if (_skipTextGenerationCoroutine != null)
            {
                StopCoroutine(_skipTextGenerationCoroutine);
            }
        }

        public void OpenDialogueWindow()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Dialogue);
            
            dialogueWindow.SetActive(true);
            
            _completedTextGeneration = true;
            _canSkipTextGeneration = false;
            _currentLine = 0;

            GenerateNextLine();
        }
        
        private void CloseDialogueWindow()
        {
            _currentLine = 0;
            
            PlayerInputManager.Instance.SetContext(_closingInputContext);
            
            dialogueWindow.SetActive(false);

            onDialogueComplete?.Invoke();
        }
    }
}
