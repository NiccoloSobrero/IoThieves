using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using Enemy;
using Interactables;
using Managers;
using Minigames;
using Minigames.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Levels
{
    public class TutorialLevelManager : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");
        private TutorialObjective _objective;
        [SerializeField] private Transform cameraTarget;
        
        [Header("Step 0")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue0;
        [SerializeField] private GameObject step0Trigger;
        [SerializeField] private GameObject codePadInteractable;
        [SerializeField] private AnimationCurve targetMovementCurve;
        [SerializeField] private LayerMask greenInteractableLayer;
        [SerializeField] private float timeToPan;
        [SerializeField] private float timeStay;
        
        [Space]
        [Header("Step 1")]
        [SerializeField] private GameObject printerInteractable;
        [SerializeField] private LayerMask blueInteractableLayer;

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
        [SerializeField] private InteractToMinigames codePadMinigame;
        
        [Space]
        [Header("Step 5")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue5;
        
        [Space]
        [Header("Step 6")]
        [SerializeField] private GameObject pcInteractable;
        [SerializeField] private LayerMask defaultLayer;
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

        [Space] [Header("Step 11")] 
        [SerializeField] private DialogueLinesScrObj tutorialDialogue11;
        [SerializeField] private Animator shuttersAnim;
        [SerializeField] private AttractableEnemy enemyCamera;
        [SerializeField] private InteractToAttract hackableCube;
        [SerializeField] private Transform shutterCameraTransform;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerTargetPos;
        [SerializeField] private float playerTimeToMove;
        
        [Space]
        [Header("Step 12")]
        [SerializeField] private DialogueLinesScrObj tutorialDialogue12;
        [SerializeField] private GameObject endLevelTrigger;
        [SerializeField] private GameObject exitCollider;
        
        private List<UnityAction> _steps = new();
        
        private int _currentStep;

        private void Awake()
        {
            Init();
        }
        
        private void Start()
        {
            NextObjective();
        }

        private void Init()
        {
            _objective = new TutorialObjective(this);
            
            step0Trigger.GetComponent<LevelTrigger>().Init(this);
            step0Trigger.SetActive(false);
            
            step2Trigger.GetComponent<LevelTrigger>().Init(this);
            step2Trigger.SetActive(false);
            
            endLevelTrigger.GetComponent<LevelTrigger>().Init(this);
            endLevelTrigger.SetActive(false);
            
            hackableCube.gameObject.SetActive(false);
            
            _currentStep = 0;
            
            // Build steps
            _steps = new()
            {
                Step_0, Step_1,
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

        // Intro dialogue + activate trigger for next step
        private void Step_0()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue0, InputContext.Disabled, _objective);
            DialogueManager.Instance.OpenDialogueWindow();

            step0Trigger.SetActive(true);
        }

        // Move camera to target and back to player
        private void Step_1()
        {
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            StartCoroutine(MoveCameraTo(codePadInteractable.transform.position, timeToPan, timeStay));
        }

        private IEnumerator MoveCameraTo(Vector3 target, float panTime, float stayTime)
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
            
            codePadInteractable.layer = (int)Mathf.Log(greenInteractableLayer.value, 2);
            
            yield return new WaitForSeconds(stayTime);
            
            eval = 0f;
            
            while (eval < 1f)
            {
                cameraTarget.position = Vector3.Lerp(targetPos, finalPos, targetMovementCurve.Evaluate(eval));
                eval += Time.deltaTime / panTime;
                
                yield return null;
            }
            
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
        }

        private void Step_2()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue2);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Set up printer
            printerInteractable.layer = (int)Mathf.Log(blueInteractableLayer.value, 2);
            printerInteractable.GetComponent<InteractToCreateDistraction>().SetObjective(_objective);
        }

        private void Step_3()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue3);
            DialogueManager.Instance.OpenDialogueWindow();
            
            step2Trigger.SetActive(true);
        }

        private void Step_4()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue4);
            DialogueManager.Instance.OpenDialogueWindow();
            
            codePadMinigame.SetOnInteractObjective(_objective);
        }

        private void Step_5()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue5, tutorialObjective:_objective);
            DialogueManager.Instance.OpenDialogueWindow();
        }

        private void Step_6()
        {
            // Close Pad password minigame and highlight pc and disable code pad minigame
            codePadMinigame.gameObject.layer = (int)Mathf.Log(defaultLayer.value, 2);
            MinigamesManager.Instance.CloseMinigameWindow();
            pcInteractable.layer = (int)Mathf.Log(greenInteractableLayer.value, 2);
            
            // Set up pc minigame to activate next objective when opening each minigame
            // Then set up desktop navigation minigame to display last dialogue
            MinigamesManager.Instance.SetPasswordDecryptionMinigameOpenObjective(passwordDecryptionMinigame, _objective);
            MinigamesManager.Instance.SetDesktopMinigameOpenObjective(desktopNavigationMinigame, _objective);
            desktopMinigame.SetOnOpenFolderWithTargetObjective(_objective);
        }
        
        private void Step_7()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue7, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
        }

        private void Step_8()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue8, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
        }

        private void Step_9()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue9, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            DownloadsManager.Instance.SetOnDownloadTargetFile(_objective);
        }
        
        private void Step_10()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue10, InputContext.Minigame);
            DialogueManager.Instance.OpenDialogueWindow();
            
            codePadInteractable.layer = (int)Mathf.Log(greenInteractableLayer.value, 2);
            safeCodeMinigame.SetOnGuessedPassword(_objective);
        }

        private void Step_11()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue11);
            DialogueManager.Instance.OpenDialogueWindow();

            Vector3 targetPos = shutterCameraTransform.position;
            targetPos.y = 0;
            
            enemyCamera.MoveTowards(targetPos);
            
            hackableCube.gameObject.SetActive(true);
            hackableCube.SetOnInteractObjective(_objective);

            shuttersAnim.SetTrigger(Open);
        }

        private void Step_12()
        {
            DialogueManager.Instance.SetDialogue(tutorialDialogue12);
            DialogueManager.Instance.OpenDialogueWindow();
            
            endLevelTrigger.SetActive(true);
            exitCollider.SetActive(false);
        }

        private void EndLevel()
        {
            Debug.Log("EndLevel");
        }
    }
}
