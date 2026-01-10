using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using Enemy;
using Events;
using Interactables;
using Managers;
using Minigames;
using Minigames.ScriptableObjects;
using Player;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Levels
{
    public class TutorialLevelManager : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");

        [Header("Objectives")] 
        [SerializeField] private LevelObjectivesUI levelObjectivesUI;
        [SerializeField] private LevelObjectiveScrObj objective1;
        [SerializeField] private LevelObjectiveScrObj objective2;
        [SerializeField] private LevelObjectiveScrObj objective3;
        [SerializeField] private LevelObjectiveScrObj objective4;
        
        [Header("Miscellaneous")]
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private EndLevelMenuController endLevelMenuController;
        [SerializeField] private LayerMask interactableLayer;
        
        [SerializeField] private LayerMask defaultLayer;

        [Header("Step reset")] 
        [SerializeField] private DialogueLinesScrObj onStepReset;
        [SerializeField] private FadeInOut fadeScreen;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeStayTime;
        [SerializeField] private float fadeOutTime;
        
        [Header("Step 0")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue0;
        [SerializeField] private GameObject step0Trigger;
        [SerializeField] private GameObject codePadInteractable;
        [SerializeField] private GameObject codePadInteractableOutline;
        [SerializeField] private InteractableTriggerArea codePadInteractableTriggerArea;
        [SerializeField] private AnimationCurve targetMovementCurve;
        [SerializeField] private float timeToPan;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioPlayerUI audioPlayerUI;
        
        [Space]
        [Header("Step 1")]
        [SerializeField] private InteractToCreateDistraction printerInteractable;
        [SerializeField] private GameObject printerInteractableOutline;
        [SerializeField] private InteractableTriggerArea printerInteractableTriggerArea;

        [Space]
        [Header("Step 2")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue2;
        [SerializeField] private GameObject step2Trigger;
        
        [Space]
        [Header("Step 3")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue3;
        
        [Space]
        [Header("Step 4")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue4;
        [SerializeField] private AttractableEnemy cctvAttractable;
        [SerializeField] private InteractToMinigames codePadMinigame;
        
        [Space]
        [Header("Step 5")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue5;
        
        [Space]
        [Header("Step 6")]
        [SerializeField] private GameObject pcInteractable;
        [SerializeField] private GameObject pcInteractableOutline;
        [SerializeField] private MinigameScrObj passwordDecryptionMinigame;
        [SerializeField] private MinigameScrObj desktopNavigationMinigame;
        [SerializeField] private DesktopMinigame desktopMinigame;
        
        [Space]
        [Header("Step 7")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue7;
        
        [Space]
        [Header("Step 8")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue8;
        
        [Space]
        [Header("Step 9")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue9;
        
        [Space]
        [Header("Step 10")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue10;
        [SerializeField] private SafeCodeMinigame safeCodeMinigame;

        [Space]
        [Header("Step 11")] 
        [SerializeField] private DialogueLinesScrObj tutorialDialogue11;
        [SerializeField] private Animator shuttersAnim;
        [SerializeField] private AttractableEnemy enemyCamera;
        [SerializeField] private InteractToAttract hackableWasherInteractable;
        [SerializeField] private GameObject hackableWasherOutline;
        [SerializeField] private InteractableTriggerArea hackableWasherTriggerArea;
        [SerializeField] private Transform shutterCameraTransform;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private SimpleCharacterController playerController;
        [SerializeField] private Transform playerTargetPos;
        
        [Space]
        [Header("Step 12")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue12;
        [SerializeField] private GameObject endLevelTrigger;
        [SerializeField] private GameObject exitCollider;
        [SerializeField] private GameObject lastHackableTriggerArea;
        
        private List<UnityAction> _steps = new();
        private Vector3 _lastValidPosition;
        
        private int _currentStep;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            EventsManager.OnTriggerOnLineEnd.AddListener(NextObjective);
            EventsManager.OnTriggerOnLineStart.AddListener(NextObjective);
            EventsManager.OnTriggerOnContinueDialogue.AddListener(NextObjective);
        }

        private void OnDisable()
        {
            EventsManager.OnTriggerOnLineEnd.RemoveListener(NextObjective);
            EventsManager.OnTriggerOnLineStart.RemoveListener(NextObjective);
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(NextObjective);
        }

        private void Start()
        {
            NextObjective();
        }

        private void Init()
        {
            audioPlayerUI.gameObject.SetActive(false);
            
            step0Trigger.GetComponent<TutorialLevelTrigger>().Init(this);
            step0Trigger.SetActive(false);
            
            step2Trigger.GetComponent<TutorialLevelTrigger>().Init(this);
            step2Trigger.SetActive(false);
            
            endLevelTrigger.GetComponent<TutorialLevelTrigger>().Init(this);
            endLevelTrigger.SetActive(false);
            
            printerInteractableOutline.SetActive(false);
            pcInteractableOutline.SetActive(false);
            codePadInteractableOutline.SetActive(false);
            hackableWasherOutline.SetActive(false);
            
            lastHackableTriggerArea.SetActive(false);
            
            _currentStep = 0;
            
            // Build steps
            _steps = new()
            {
                Step_0, Step_1, Step_1_2, Step_1_3,
                Step_2, Step_3,
                Step_4, Step_5,
                Step_6, Step_7,
                Step_8, Step_9,
                Step_10, Step_11,
                Step_12, EndLevel
            };
        }
        
        public void NextObjective()
        {
            if (_currentStep < _steps.Count)
            {
                _steps[_currentStep].Invoke();

                _currentStep++;
            }
        }

        private void Step_0()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue0, InputContext.Disabled);
            DialogueManager.Instance.OpenDialogueWindow();

            step0Trigger.SetActive(true);

            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.AddObjective(objective1);
        }

        private void Step_1()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            StartCoroutine(MoveCameraToTarget(codePadInteractable.transform.position, timeToPan));
            
            _lastValidPosition = playerTransform.position;
        }

        private void Step_1_2()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            StartCoroutine(MoveCameraBack(playerTransform.position, timeToPan));
            
            _lastValidPosition = playerTransform.position;
        }

        private IEnumerator MoveCameraToTarget(Vector3 target, float panTime)
        {
            Vector3 targetPos = target;
            targetPos.y = cameraTarget.position.y;
            
            Vector3 finalPos = cameraTarget.position;

            float eval = 0f;

            while (eval < 1f)
            {
                cameraTarget.position = Vector3.Lerp(finalPos, targetPos, targetMovementCurve.Evaluate(eval));
                eval += Time.deltaTime / panTime;
                
                yield return null;
            }
            
            codePadInteractable.layer = (int)Mathf.Log(interactableLayer.value, 2);
            codePadInteractableOutline.SetActive(true);
            
            codePadInteractableTriggerArea.ManualUpdate();
            
            PlayerInputManager.Instance.SetContext(InputContext.Dialogue);
        }
        
        private IEnumerator MoveCameraBack(Vector3 target, float panTime)
        {
            Vector3 targetPos = cameraTarget.position;

            Vector3 finalPos = target;

            float eval = 0f;
            
            while (eval < 1f)
            {
                cameraTarget.position = Vector3.Lerp(targetPos, finalPos, targetMovementCurve.Evaluate(eval));
                eval += Time.deltaTime / panTime;
                
                yield return null;
            }
            
            codePadInteractableTriggerArea.ManualUpdate();
            
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
        }
        
        private void Step_1_3()
        {
            audioManager.Play();
            audioPlayerUI.gameObject.SetActive(true);
            
            audioPlayerUI.SetOpen(true);
        }

        private void Step_2()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue2);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Set up printer
            printerInteractable.gameObject.layer = (int)Mathf.Log(interactableLayer.value, 2);
            printerInteractableOutline.SetActive(true);
            printerInteractableTriggerArea.ManualUpdate();
            printerInteractable.onPickUpThrowable.AddListener(NextStepOnPickUpThrowable);
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective1);
        }

        private void Step_3()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue3);
            DialogueManager.Instance.OpenDialogueWindow();
            
            step2Trigger.SetActive(true);
            
            _lastValidPosition = playerTransform.position;
        }

        private void Step_4()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue4);
            DialogueManager.Instance.OpenDialogueWindow();
            
            codePadMinigame.onInteract.AddListener(NextStepOnCodePadInteract);
            
            // Move cctv
            cctvAttractable.MoveTowards(step2Trigger.transform.position);
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective1);
        }

        private void Step_5()
        {
            DialogueManager.Instance.onDialogueComplete.AddListener(NextStepOnDialogueCompleted);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue5);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective1);
            levelObjectivesUI.AddObjective(objective2);
        }

        private void Step_6()
        {
            // Close Pad password minigame and highlight pc and disable code pad minigame
            codePadMinigame.gameObject.layer = (int)Mathf.Log(defaultLayer.value, 2);
            codePadInteractableOutline.SetActive(false);
            MinigamesManager.Instance.CloseMinigameWindow();
            pcInteractable.layer = (int)Mathf.Log(interactableLayer.value, 2);
            pcInteractableOutline.SetActive(true);
            
            // Set up pc minigame to activate next objective when opening each minigame
            // Then set up desktop navigation minigame to display last dialogue
            MinigamesManager.Instance.onDesktopMinigameOpened.AddListener(OnDesktopMinigameOpened);
            MinigamesManager.Instance.onPasswordDecryptionMinigameOpened.AddListener(OnPasswordDecryptionMinigameOpen);
            
            desktopMinigame.onOpenFolderWithTarget.AddListener(NextStepOnOpenFolderWithTarget);
            
            _lastValidPosition = playerTransform.position;
        }
        
        private void Step_7()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue7, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
        }

        private void Step_8()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue8, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
        }

        private void Step_9()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue9, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(NextStepOnDownloadTargetFile);
            
            _lastValidPosition = playerTransform.position;
        }
        
        private void Step_10()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue10, PlayerInputManager.Instance.CurrentContext);
            DialogueManager.Instance.OpenDialogueWindow();
            
            codePadInteractable.layer = (int)Mathf.Log(interactableLayer.value, 2);
            codePadInteractableOutline.SetActive(true);
            safeCodeMinigame.onGuessedPassword.AddListener(NextStepOnSafeMinigamePasswordGuessed);
            
            _lastValidPosition = playerTransform.position;
            
            EventsManager.OnFoundCodePadSolution.Invoke();
            
            levelObjectivesUI.UpdateObjective(objective2);
            levelObjectivesUI.AddObjective(objective3);
        }

        private void Step_11()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue11);
            DialogueManager.Instance.OpenDialogueWindow();

            Vector3 targetPos = shutterCameraTransform.position;
            targetPos.y = 0;
            
            enemyCamera.MoveTowards(targetPos);
            
            hackableWasherOutline.SetActive(true);
            hackableWasherTriggerArea.ManualUpdate();
            
            hackableWasherInteractable.onInteract.AddListener(NextStepOnHackableWasherInteract);

            shuttersAnim.SetTrigger(Open);
            
            // Move player to safe spot
            playerController.MoveTowards(playerTargetPos.position);
            
            _lastValidPosition = playerTargetPos.position;
            
            lastHackableTriggerArea.SetActive(true);
            
            levelObjectivesUI.UpdateObjective(objective3);
            levelObjectivesUI.AddObjective(objective4);
        }

        private void Step_12()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue12);
            DialogueManager.Instance.OpenDialogueWindow();
            
            endLevelTrigger.SetActive(true);
            exitCollider.SetActive(false);
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective4);
        }

        private void EndLevel()
        {
            EventsManager.OnEndLevel?.Invoke();
        }

        public void ResetToCurrentStep()
        {
            StartCoroutine(ResetToCurrentStepCoroutine());
        }

        private IEnumerator ResetToCurrentStepCoroutine()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            fadeScreen.gameObject.SetActive(true);
            fadeScreen.StartFade(fadeInTime, fadeStayTime, fadeOutTime);
            
            yield return new WaitForSeconds(fadeInTime);
            
            playerTransform.position = _lastValidPosition;
            
            yield return new WaitForSeconds(fadeStayTime + fadeOutTime);
            
            DialogueManager.Instance.SetDialogue(onStepReset);
            DialogueManager.Instance.OpenDialogueWindow();
        }

        #region Event Callbacks

        private void NextStepOnDialogueCompleted()
        {
            DialogueManager.Instance.onDialogueComplete.RemoveListener(NextStepOnDialogueCompleted);
            NextObjective();
        }

        private void NextStepOnPickUpThrowable()
        {
            printerInteractable.onPickUpThrowable.RemoveListener(NextStepOnPickUpThrowable);
            NextObjective();
        }

        private void NextStepOnCodePadInteract()
        {
            codePadMinigame.onInteract.RemoveListener(NextStepOnCodePadInteract);
            NextObjective();
        }
        
        private void OnDesktopMinigameOpened(MinigameScrObj opened)
        {
            if (opened == desktopNavigationMinigame)
            {
                MinigamesManager.Instance.onDesktopMinigameOpened.RemoveListener(OnDesktopMinigameOpened);
                NextObjective();
            }
        }

        private void OnPasswordDecryptionMinigameOpen(MinigameScrObj opened)
        {
            if (opened == passwordDecryptionMinigame)
            {
                MinigamesManager.Instance.onPasswordDecryptionMinigameOpened.RemoveListener(OnPasswordDecryptionMinigameOpen);
                NextObjective();
            }
        }

        private void NextStepOnOpenFolderWithTarget()
        {
            desktopMinigame.onOpenFolderWithTarget.RemoveListener(NextStepOnOpenFolderWithTarget);
            NextObjective();
        }

        private void NextStepOnDownloadTargetFile()
        {
            DownloadsManager.Instance.onDownloadTargetFile.RemoveListener(NextStepOnDownloadTargetFile);
            NextObjective();
        }

        private void NextStepOnSafeMinigamePasswordGuessed()
        {
            safeCodeMinigame.onGuessedPassword.RemoveListener(NextStepOnSafeMinigamePasswordGuessed);
            NextObjective();
        }

        private void NextStepOnHackableWasherInteract()
        {
            hackableWasherInteractable.onInteract.RemoveListener(NextStepOnHackableWasherInteract);
            NextObjective();
        }
        
        #endregion
    }
}
