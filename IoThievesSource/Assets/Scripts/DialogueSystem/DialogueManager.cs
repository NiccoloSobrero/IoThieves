using System.Collections;
using Levels;
using Managers;
using TMPro;
using UnityEngine;
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
        private const float SkipTextGenerationDelay = 0.5f;
        
        private int _currentLine;
        private bool _completedTextGeneration;
        private bool _canSkipTextGeneration;
        
        private Coroutine _textGenerationCoroutine;
        private Coroutine _skipTextGenerationCoroutine;
        
        private TutorialObjective _tutorialObjective;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _completedTextGeneration = true;
            _canSkipTextGeneration = false;
        }

        public void SetDialogue(DialogueLinesScrObj dialogueLinesScrObj, TutorialObjective tutorialObjective = null)
        {
            _dialogueLinesScrObj = dialogueLinesScrObj;
            _tutorialObjective = tutorialObjective;
        }

        public void GenerateNextLine()
        {
            if (_completedTextGeneration)
            {
                if (_currentLine < _dialogueLinesScrObj.dialogueLines.Length)
                {
                    dialogue.text = _dialogueLinesScrObj.dialogueLines[_currentLine].text;
                    speakerIcon.sprite = _dialogueLinesScrObj.dialogueLines[_currentLine].icon;
                    speakerName.text = _dialogueLinesScrObj.dialogueLines[_currentLine].speakerName;

                    dialogue.maxVisibleCharacters = 0;
                    textCompletedObj.SetActive(false);

                    _completedTextGeneration = false;
                    
                    TryStoppingCoroutines();

                    _textGenerationCoroutine = StartCoroutine(ShowWithDelay(CharactersDelay, dialogue.text.Length));
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

        private IEnumerator ShowWithDelay(float letterDelay, int totalCharacters)
        {
            int currentShownCharacters = 0;

            while (currentShownCharacters < totalCharacters)
            {
                currentShownCharacters++;
                dialogue.maxVisibleCharacters = currentShownCharacters;
                
                yield return new WaitForSeconds(letterDelay);
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
            
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
            
            dialogueWindow.SetActive(false);

            if (_tutorialObjective != null)
            {
                _tutorialObjective.Complete();
                _tutorialObjective = null;
            }
        }
    }
}
