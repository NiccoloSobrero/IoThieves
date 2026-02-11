using System.Collections;
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
        [SerializeField] private LevelTrigger step0LevelTrigger;
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
        [SerializeField] private LevelTrigger step2LevelTrigger;
        
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
        [SerializeField] private LevelTrigger endGameLevelTrigger;
        [SerializeField] private GameObject exitCollider;
        [SerializeField] private GameObject lastHackableTriggerArea;
        
        private Vector3 _lastValidPosition;
        
        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            SetUp();
        }

        private void Init()
        {
            audioPlayerUI.gameObject.SetActive(false);
            
            // Disable all level triggers and outlines
            step0LevelTrigger.gameObject.SetActive(false);
            step2LevelTrigger.gameObject.SetActive(false);
            endGameLevelTrigger.gameObject.SetActive(false);
            
            printerInteractableOutline.SetActive(false);
            pcInteractableOutline.SetActive(false);
            codePadInteractableOutline.SetActive(false);
            hackableWasherOutline.SetActive(false);

            printerInteractable.gameObject.layer = (int)Mathf.Log(defaultLayer.value, 2);
            
            lastHackableTriggerArea.SetActive(false);
        }

        private void SetUp()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue0);
            DialogueManager.Instance.OpenDialogueWindow();

            _lastValidPosition = playerTransform.position;
            
            EventsManager.OnTriggerOnLineStart.AddListener(MoveCameraToCodePad);
        }
        
        private void MoveCameraToCodePad()
        {
            EventsManager.OnTriggerOnLineStart.RemoveListener(MoveCameraToCodePad);
            
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            StartCoroutine(MoveCameraToTarget(codePadInteractable.transform.position, timeToPan));
            
            _lastValidPosition = playerTransform.position;
            
            EventsManager.OnTriggerOnContinueDialogue.AddListener(MoveCameraBackToPlayer);
        }
        
        private void MoveCameraBackToPlayer()
        {
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(MoveCameraBackToPlayer);
            
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            StartCoroutine(MoveCameraBack(playerTransform.position, timeToPan));
            
            _lastValidPosition = playerTransform.position;
            
            EventsManager.OnTriggerOnContinueDialogue.AddListener(OpenAudioPlayerUI);
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
            
            PlayerInputManager.Instance.SetContext(InputContext.Dialogue);
        }
        
        private void OpenAudioPlayerUI()
        {
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(OpenAudioPlayerUI);
            
            levelObjectivesUI.AddObjective(objective1);
            
            audioManager.Play();
            audioPlayerUI.gameObject.SetActive(true);
            
            audioPlayerUI.SetOpen(true);
            
            // Enable trigger for next step
            step0LevelTrigger.gameObject.SetActive(true);
            step0LevelTrigger.onTriggerEnter.AddListener(OnStep2TriggerEnter);
        }
        
        private void OnStep2TriggerEnter()
        {
            step2LevelTrigger.onTriggerEnter.RemoveListener(OnStep2TriggerEnter);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue2);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Set up printer
            printerInteractable.gameObject.layer = (int)Mathf.Log(interactableLayer.value, 2);
            printerInteractableOutline.SetActive(true);
            printerInteractableTriggerArea.ManualUpdate();
            printerInteractable.onPickUpThrowable.AddListener(OnPickUpThrowable);
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective1);
        }
        
        private void OnPickUpThrowable()
        {
            printerInteractable.onPickUpThrowable.RemoveListener(OnPickUpThrowable);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue3);
            DialogueManager.Instance.OpenDialogueWindow();
            
            step2LevelTrigger.gameObject.SetActive(true);
            
            _lastValidPosition = playerTransform.position;
            
            step2LevelTrigger.onTriggerEnter.AddListener(OnThrowableTriggerEnter);
        }
        
        private void OnThrowableTriggerEnter()
        {
            step2LevelTrigger.onTriggerEnter.RemoveListener(OnThrowableTriggerEnter);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue4);
            DialogueManager.Instance.OpenDialogueWindow();
            
            codePadMinigame.onInteract.AddListener(OnCodePadInteract);
            
            // Move cctv
            cctvAttractable.MoveTowards(step2LevelTrigger.transform.position);
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective1);
        }

        private void OnCodePadInteract()
        {
            codePadMinigame.onInteract.RemoveListener(OnCodePadInteract);
            
            DialogueManager.Instance.onDialogueComplete.AddListener(OnCodePadDialogueComplete);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue5);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective1);
            levelObjectivesUI.AddObjective(objective2);
        }
        
        private void OnCodePadDialogueComplete()
        {
            DialogueManager.Instance.onDialogueComplete.RemoveListener(OnCodePadDialogueComplete);
            
            // Close Pad password minigame and highlight pc and disable code pad minigame
            codePadMinigame.gameObject.layer = (int)Mathf.Log(defaultLayer.value, 2);
            codePadInteractableOutline.SetActive(false);
            MinigamesManager.Instance.CloseMinigameWindow();
            pcInteractable.layer = (int)Mathf.Log(interactableLayer.value, 2);
            pcInteractableOutline.SetActive(true);
            
            // Set up pc minigame to activate next objective when opening each minigame
            // Then set up desktop navigation minigame to display last dialogue
            MinigamesManager.Instance.onMinigameOpened.AddListener(OnPasswordDecryptionMinigameOpen);
            
            _lastValidPosition = playerTransform.position;
        }

        private void OnPasswordDecryptionMinigameOpen()
        {
            MinigamesManager.Instance.onMinigameOpened.RemoveListener(OnPasswordDecryptionMinigameOpen);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue7, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
            
            MinigamesManager.Instance.onMinigameOpened.AddListener(OnDesktopMinigameOpened);
        }

        private void OnDesktopMinigameOpened()
        {
            MinigamesManager.Instance.onMinigameOpened.RemoveListener(OnDesktopMinigameOpened);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue8, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
            
            desktopMinigame.onOpenFolderWithTarget.AddListener(OnOpenFileWithTarget);
        }
        
        private void OnOpenFileWithTarget()
        {
            desktopMinigame.onOpenFolderWithTarget.RemoveListener(OnOpenFileWithTarget);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue9, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            _lastValidPosition = playerTransform.position;
            
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(OnDownloadTargetFile);
        }

        private void OnDownloadTargetFile()
        {
            DownloadsManager.Instance.onDownloadTargetFile.RemoveListener(OnDownloadTargetFile);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue10, PlayerInputManager.Instance.CurrentContext);
            DialogueManager.Instance.OpenDialogueWindow();
            
            codePadInteractable.layer = (int)Mathf.Log(interactableLayer.value, 2);
            codePadInteractableOutline.SetActive(true);
            safeCodeMinigame.onGuessedPassword.AddListener(OnCodePadPasswordGuessed);
            
            _lastValidPosition = playerTransform.position;
            
            EventsManager.OnFoundCodePadSolution.Invoke();
            
            levelObjectivesUI.UpdateObjective(objective2);
            levelObjectivesUI.AddObjective(objective3);
        }

        private void OnCodePadPasswordGuessed()
        {
            safeCodeMinigame.onGuessedPassword.RemoveListener(OnCodePadPasswordGuessed);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue11);
            DialogueManager.Instance.OpenDialogueWindow();

            Vector3 targetPos = shutterCameraTransform.position;
            targetPos.y = 0;
            
            enemyCamera.MoveTowards(targetPos);
            
            hackableWasherOutline.SetActive(true);
            hackableWasherTriggerArea.ManualUpdate();
            
            hackableWasherInteractable.onInteract.AddListener(OnHackableWasherInteract);

            shuttersAnim.SetTrigger(Open);
            
            // Move player to safe spot
            playerController.MoveTowards(playerTargetPos.position);
            
            _lastValidPosition = playerTargetPos.position;
            
            lastHackableTriggerArea.SetActive(true);
            
            levelObjectivesUI.UpdateObjective(objective3);
            levelObjectivesUI.AddObjective(objective4);
        }

        private void OnHackableWasherInteract()
        {
            hackableWasherInteractable.onInteract.RemoveListener(OnHackableWasherInteract);
            
            DialogueManager.Instance.SetDialogue(tutorialDialogue12);
            DialogueManager.Instance.OpenDialogueWindow();
            
            endGameLevelTrigger.gameObject.SetActive(true);
            endGameLevelTrigger.onTriggerEnter.AddListener(EndLevel);
            
            exitCollider.SetActive(false);
            
            _lastValidPosition = playerTransform.position;
            
            levelObjectivesUI.UpdateObjective(objective4);
        }

        private void EndLevel()
        {
            endGameLevelTrigger.onTriggerEnter.RemoveListener(EndLevel);
            
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
    }
}
