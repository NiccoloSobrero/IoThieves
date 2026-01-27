using System.Collections;
using DialogueSystem;
using Events;
using Interactables;
using Managers;
using Minigames;
using Minigames.ScriptableObjects;
using UI;
using UnityEngine;

namespace Levels
{
    public class HrOfficeLevelManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelObjectivesUI levelObjectivesUI;
        
        [Header("Objectives")]
        [SerializeField] private LevelObjectiveScrObj getBossAddress;
        [SerializeField] private LevelObjectiveScrObj fileComplaint;
        [SerializeField] private LevelObjectiveScrObj getOut;

        [Header("Unlock door")]
        [SerializeField] private InteractToInvokeEvent keyInteractable;
        [SerializeField] private DoorTrigger bossDoorTrigger;

        [SerializeField] private DialogueLinesScrObj introDialogue;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform bossOfficeCameraTarget;
        [SerializeField] private AnimationCurve cameraMoveCurve;

        [SerializeField] private DialogueLinesScrObj getKeyDialogue;
        
        [SerializeField] private LevelTrigger enterCanteenTrigger;
        [SerializeField] private DialogueLinesScrObj enterCanteenDialogue;

        [SerializeField] private DialogueLinesScrObj obtainedKeyDialogue;

        private const float CameraTimeToTarget = 1f;
        
        [Header("Get address")]
        [SerializeField] private DesktopMinigame bossAddressMinigame;
        [SerializeField] private MinigameScrObj bossDesktopNavigationMinigame;
        
        [Header("File complaint")]
        [SerializeField] private TypeWriteMinigame typeWriteMinigame;

        [Header("Get out")]
        [SerializeField] private DialogueLinesScrObj exitDialogue;
        [SerializeField] private LevelTrigger endLevelTrigger;

        private bool _completedFileComplaint;
        private bool _obtainedBossAddress;
        
        /*
         * Flow:
         *      - Initial dialogues to explain the target is the boss office (Reference to HR complaint pc)
         *      - Pan camera back and forth (camera move -> dialogues -> comes back -> more dialogues) like tutorial
         *      - (Free movement)
         *      - Entering the boss office's door trigger (locked) starts a new dialogue (FIND THE KEY)
         *      - Obtain key
         *      - Dialogue (go back to door)
         *      - Open pc, download target file (Check for last dialogue set up)
         *      - Complete hr complaint (could have been done before)
         *      - Last dialogue (When both objective have been completed)
         *      - Get out
         */
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            // Initialization
            enterCanteenTrigger.gameObject.SetActive(false);
            endLevelTrigger.gameObject.SetActive(false);

            _completedFileComplaint = false;
            _obtainedBossAddress = false;
            
            // First objectives set up
            bossDoorTrigger.SetLocked(true);
            keyInteractable.SetEnabled(false);
            
            // Start dialogue
            DialogueManager.Instance.SetDialogue(introDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            EventsManager.OnTriggerOnContinueDialogue.AddListener(MoveCameraToBossOffice);
        }

        private void MoveCameraToBossOffice()
        {
            StartCoroutine(MoveCameraToCor(cameraTarget, bossOfficeCameraTarget));
            
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(MoveCameraToBossOffice);
            
            EventsManager.OnTriggerOnContinueDialogue.AddListener(MoveCameraBackToPlayer);
        }

        private void MoveCameraBackToPlayer()
        {
            StartCoroutine(MoveCameraToCor(cameraTarget, playerTransform));
            
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(MoveCameraBackToPlayer);
            
            // Add listener for writing the correct complaint
            typeWriteMinigame.onSendComplaint.AddListener(OnComplaintSent);
            
            // Add listener for boss door trigger
            bossDoorTrigger.onTriggerEnterEvent.AddListener(OnEnterBossDoorTrigger);
            
            EventsManager.OnTriggerOnLineStart.AddListener(ShowFirstObjectives);
        }
        
        private void ShowFirstObjectives()
        {
            levelObjectivesUI.AddObjective(getBossAddress);
            levelObjectivesUI.AddObjective(fileComplaint);
            
            EventsManager.OnTriggerOnLineStart.RemoveListener(ShowFirstObjectives);
        }

        private IEnumerator MoveCameraToCor(Transform cameraFollow, Transform target)
        {
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            // Lock axis y
            Vector3 originalPos = cameraFollow.position;
            Vector3 targetPos = target.position;

            float eval = 0f;
            while (eval < 1f)
            {
                cameraTarget.position = Vector3.Lerp(originalPos, targetPos, cameraMoveCurve.Evaluate(eval));
                eval += Time.deltaTime / CameraTimeToTarget;
                yield return null;
            }
            
            PlayerInputManager.Instance.SetContext(InputContext.Dialogue);
        }

        private void OnEnterBossDoorTrigger()
        {
            levelObjectivesUI.UpdateObjective(getBossAddress);
            bossDoorTrigger.onTriggerEnterEvent.RemoveListener(OnEnterBossDoorTrigger);
            
            DialogueManager.Instance.SetDialogue(getKeyDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            enterCanteenTrigger.gameObject.SetActive(true);
            enterCanteenTrigger.onTriggerEnter.AddListener(OnEnterCanteen);
        }

        private void OnEnterCanteen()
        {
            enterCanteenTrigger.gameObject.SetActive(false);
            enterCanteenTrigger.onTriggerEnter.RemoveListener(OnEnterCanteen);
            
            DialogueManager.Instance.SetDialogue(enterCanteenDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            keyInteractable.SetEnabled(true);
            keyInteractable.onInteract.AddListener(OnKeyInteract);
        }

        private void OnKeyInteract()
        {
            levelObjectivesUI.UpdateObjective(getBossAddress);
            
            keyInteractable.SetEnabled(false);
            keyInteractable.onInteract.RemoveListener(OnKeyInteract);
            
            bossDoorTrigger.SetLocked(false);
            
            DialogueManager.Instance.SetDialogue(obtainedKeyDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(OnAddressDownloaded);
        }

        private void OnComplaintSent()
        {
            typeWriteMinigame.onSendComplaint.RemoveListener(OnComplaintSent);
            
            levelObjectivesUI.UpdateObjective(fileComplaint);
            _completedFileComplaint = true;

            if (_obtainedBossAddress)
            {
                SetUpLastStep();
            }
        }

        private void OnAddressDownloaded()
        {
            levelObjectivesUI.UpdateObjective(getBossAddress);
            
            DownloadsManager.Instance.onDownloadTargetFile.RemoveListener(OnAddressDownloaded);
            _obtainedBossAddress = true;
            
            if (_completedFileComplaint)
            {
                SetUpLastStep();
            }
        }

        private void SetUpLastStep()
        {
            levelObjectivesUI.AddObjective(getOut);
            
            endLevelTrigger.gameObject.SetActive(true);
            endLevelTrigger.onTriggerEnter.AddListener(OnGetOut);
            
            DialogueManager.Instance.SetDialogue(exitDialogue, PlayerInputManager.Instance.CurrentContext);
            DialogueManager.Instance.OpenDialogueWindow();
        }

        private void OnGetOut()
        {
            levelObjectivesUI.UpdateObjective(getOut);
            endLevelTrigger.onTriggerEnter.RemoveListener(OnGetOut);
            
            // End level
            EventsManager.OnEndLevel?.Invoke();
            
        }
    }
}
