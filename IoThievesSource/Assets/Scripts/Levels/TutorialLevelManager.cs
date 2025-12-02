using DialogueSystem;
using UnityEngine;

namespace Levels
{
    public class TutorialLevelManager : MonoBehaviour
    {
        private TutorialObjective _objective;
        
        [Header("Part 1")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue1;
        
        [Header("Part 2")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue2;
        [SerializeField] private GameObject part2Trigger;

        [Header("Part 3")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue3;
        
        [Header("Part 4")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue4;
        
        [Header("Part 5")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue5;
        
        private int _currentStep;

        private void Awake()
        {
            Init();
        }
        
        private void Start()
        {
            NextObjective();
        }

        private void OnEnable()
        {
            // EventsManager.OnDialogueEnded.AddListener(NextObjective);
        }

        private void OnDisable()
        {
            // EventsManager.OnDialogueEnded.RemoveListener(NextObjective);
        }

        private void Init()
        {
            _objective = new TutorialObjective(this);
            
            part2Trigger.GetComponent<LevelTrigger>().Init(this);
            part2Trigger.SetActive(false);
            
            _currentStep = 0;
        }

        public void NextObjective()
        {
            if (_currentStep == 0)
            {
                DialogueManager.Instance.SetDialogue(tutorialDialogue1);
                DialogueManager.Instance.OpenDialogueWindow();

                part2Trigger.SetActive(true);
                
                _currentStep++;
            }
            else if (_currentStep == 1)
            {
                DialogueManager.Instance.SetDialogue(tutorialDialogue2, _objective);
                DialogueManager.Instance.OpenDialogueWindow();
                
                _currentStep++;
            }
            else if (_currentStep == 2)
            {
                DialogueManager.Instance.SetDialogue(tutorialDialogue3);
                DialogueManager.Instance.OpenDialogueWindow();

                _currentStep++;
            }
        }
    }
}
