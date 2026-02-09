using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using Events;
using Interactables;
using Managers;
using Minigames;
using UI;
using UnityEngine;

namespace Levels
{
    public class BossHouseLevelManager : LevelManager
    {
        [Header("References")]
        [SerializeField] private LevelObjectivesUI levelObjectivesUI;
        
        [Header("Objectives")]
        [SerializeField] private LevelObjectiveScrObj findChatLogs;
        [SerializeField] private LevelObjectiveScrObj downloadFileFromSonPC;
        [SerializeField] private LevelObjectiveScrObj stealValuesFromKitchen;
        [SerializeField] private LevelObjectiveScrObj stealValuesFromLivingRoom;
        [SerializeField] private LevelObjectiveScrObj stealPeluches;

        private bool _downloadedFileFromSonPC;
        private bool _stoleValuesFromKitchen;
        private bool _stoleValuesFromLivingRoom;
        private bool _stolePeluches;
        
        [Header("Intro")]
        [SerializeField] private DialogueLinesScrObj introDialogue;
        
        [SerializeField] private Transform cameraFollowTransform;
        [SerializeField] private Transform bossOfficeCameraTarget;
        [SerializeField] private AnimationCurve cameraMoveCurve;
        
        private const float CameraTimeToTarget = 1f;
        
        [Header("Boss room door trigger")]
        [SerializeField] private DialogueLinesScrObj onArriveAtDoorDialogue;
        [SerializeField] private DoorTrigger bossRoomDoorTrigger;
        
        [Header("Find door keys")]
        [SerializeField] private DialogueLinesScrObj onFoundDoorKeysDialogue;
        [SerializeField] private InteractToInvokeEvent doorKeysInteractable;
        
        [Header("Download chat logs")]
        [SerializeField] private DialogueLinesScrObj onChatDownloadDialogue;
        
        [Header("Download from son pc")]
        // Dialogue?
        
        [Header("Steal values from kitchen")]
        [SerializeField] private InteractToInvokeEvent kitchenValuesInteractable;
        
        [Header("Steal values from living room")]
        [SerializeField] private InteractToInvokeEvent livingroomValuesInteractable;
        
        [Header("Steal peluches")]
        [SerializeField] private List<InteractToInvokeEvent> peluchesInteractable;
        private int _peluchesStolenCount;

        [Header("All sub-objectives completed")] 
        [SerializeField] private DialogueLinesScrObj onAllSubObjectivesCompletedDialogue;
        
        [Header("Exit level")]
        [SerializeField] private DialogueLinesScrObj onExitLevelDialogue;
        [SerializeField] private InteractToInvokeEvent carExitInteractable;
        
        /*
         * Casa del capo:
            - PIP: ingresso --> porta boss
            - porta bloccata --> chiave magnetica sul divano --> porta boss sbloccata
            - download file --> dialoghi --> start missions:
            1) Download a file from son pc
            2) Steal values from kitchen/living room
            3) Steal peluche(s) (one or more)
            4) Download file from Boss PC

            missions completed --> go to garage and leave --> level completed
         */
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            bossRoomDoorTrigger.SetLocked(true);
            
            doorKeysInteractable.SetEnabled(false);
            
            kitchenValuesInteractable.SetEnabled(false);
            livingroomValuesInteractable.SetEnabled(false);
            foreach (var peluche in peluchesInteractable)
            {
                peluche.SetEnabled(false);
            }
            
            carExitInteractable.SetEnabled(false);
            
            _downloadedFileFromSonPC = false;
            _stoleValuesFromKitchen = false;
            _stoleValuesFromLivingRoom = false;
            _stolePeluches = false;
        }

        protected override void Start()
        {
            base.Start();
            
            Intro();
        }

        private void Intro()
        {
            // Show dialogue
            DialogueManager.Instance.SetDialogue(introDialogue, InputContext.Disabled);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Set up next step
            EventsManager.OnTriggerOnContinueDialogue.AddListener(MoveCameraToBossOffice);
        }

        private void MoveCameraToBossOffice()
        {
            StartCoroutine(MoveCameraToBossOfficeCor());
        }

        private IEnumerator MoveCameraToBossOfficeCor()
        {
            // Remove previous listener
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(MoveCameraToBossOffice);
            
            // Move camera
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            yield return StartCoroutine(MoveCameraToCor(cameraFollowTransform, bossOfficeCameraTarget));
            PlayerInputManager.Instance.SetContext(InputContext.Dialogue);

            // Set up next step
            EventsManager.OnTriggerOnContinueDialogue.AddListener(MoveCameraBackToPlayer);
        }
        
        private void MoveCameraBackToPlayer()
        {
            StartCoroutine(MoveCameraBackToPlayerCor());
        }
        
        private IEnumerator MoveCameraBackToPlayerCor()
        {
            // Remove previous listener
            EventsManager.OnTriggerOnContinueDialogue.RemoveListener(MoveCameraBackToPlayer);
            
            // Move camera
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            yield return StartCoroutine(MoveCameraToCor(cameraFollowTransform, playerTransform));
            PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
            
            // Update objectives
            levelObjectivesUI.AddObjective(findChatLogs);
            
            // Set up next step
            bossRoomDoorTrigger.onTriggerEnterEvent.AddListener(OnEnterBossRoomDoorTrigger);
        }
        
        private IEnumerator MoveCameraToCor(Transform cameraFollow, Transform target)
        {
            // Lock axis y
            Vector3 originalPos = cameraFollow.position;
            Vector3 targetPos = target.position;

            float eval = 0f;
            while (eval < 1f)
            {
                cameraFollowTransform.position = Vector3.Lerp(originalPos, targetPos, cameraMoveCurve.Evaluate(eval));
                eval += Time.deltaTime / CameraTimeToTarget;
                yield return null;
            }
        }

        private void OnEnterBossRoomDoorTrigger()
        {
            SaveLastValidPosition();
            
            // Remove previous listener
            bossRoomDoorTrigger.onTriggerEnterEvent.RemoveListener(OnEnterBossRoomDoorTrigger);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onArriveAtDoorDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            // Set up next step
            doorKeysInteractable.SetEnabled(true);
            doorKeysInteractable.onInteract.AddListener(OnInteractWithDoorKeys);
        }
        
        private void OnInteractWithDoorKeys()
        {
            SaveLastValidPosition();
            
            // Remove previous listener
            doorKeysInteractable.onInteract.RemoveListener(OnInteractWithDoorKeys);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onFoundDoorKeysDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            // Unlock boss room door
            bossRoomDoorTrigger.SetLocked(false);
            
            // Set up next step
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(OnChatLogsDownloaded);
        }
        
        private void OnChatLogsDownloaded()
        {
            SaveLastValidPosition();
            
            // Remove previous listener
            DownloadsManager.Instance.onDownloadTargetFile.RemoveListener(OnChatLogsDownloaded);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onChatDownloadDialogue, PlayerInputManager.Instance.CurrentContext);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            // Add sub-objectives
            levelObjectivesUI.AddObjective(downloadFileFromSonPC);
            levelObjectivesUI.AddObjective(stealValuesFromKitchen);
            levelObjectivesUI.AddObjective(stealValuesFromLivingRoom);
            levelObjectivesUI.AddObjective(stealPeluches);
            
            // Move findChatLogs to the end of the list
            levelObjectivesUI.ForceAsFirstSibling(findChatLogs);
            
            // Set up next steps for sub-objectives
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(OnDownloadedFileFromSonPC);
            
            kitchenValuesInteractable.SetEnabled(true);
            kitchenValuesInteractable.onInteract.AddListener(OnInteractWithKitchenValues);
            
            livingroomValuesInteractable.SetEnabled(true);
            livingroomValuesInteractable.onInteract.AddListener(OnInteractWithLivingroomValues);

            foreach (var peluche in peluchesInteractable)
            {
                peluche.SetEnabled(true);
                peluche.onInteract.AddListener(OnInteractWithPeluches);
            }
        }
        
        private void OnDownloadedFileFromSonPC()
        {
            levelObjectivesUI.UpdateObjective(downloadFileFromSonPC);
            
            DownloadsManager.Instance.onDownloadTargetFile.RemoveListener(OnChatLogsDownloaded);
            
            _downloadedFileFromSonPC = true;
            
            CheckIfAllSubObjectivesCompleted();
        }
        
        private void OnInteractWithKitchenValues()
        {
            kitchenValuesInteractable.onInteract.RemoveListener(OnInteractWithKitchenValues);
            
            levelObjectivesUI.UpdateObjective(stealValuesFromKitchen);
            
            _stoleValuesFromKitchen = true;
            
            CheckIfAllSubObjectivesCompleted();
        }
        
        private void OnInteractWithLivingroomValues()
        {
            livingroomValuesInteractable.onInteract.RemoveListener(OnInteractWithLivingroomValues);
            
            levelObjectivesUI.UpdateObjective(stealValuesFromLivingRoom);

            _stoleValuesFromLivingRoom = true;
            
            CheckIfAllSubObjectivesCompleted();
        }
        
        private void OnInteractWithPeluches()
        {
            _peluchesStolenCount++;
            levelObjectivesUI.UpdateObjective(stealPeluches);
            
            if (_peluchesStolenCount >= peluchesInteractable.Count)
            {
                // All peluches stolen, remove listeners
                foreach (var peluche in peluchesInteractable)
                {
                    peluche.onInteract.RemoveListener(OnInteractWithPeluches);
                }

                _stolePeluches = true;
                
                CheckIfAllSubObjectivesCompleted();
            }
        }
        
        private void CheckIfAllSubObjectivesCompleted()
        {
            if (_downloadedFileFromSonPC && _stoleValuesFromKitchen && _stoleValuesFromLivingRoom && _stolePeluches)
            {
                SetUpLevelExit();
            }
        }

        private void SetUpLevelExit()
        {
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onAllSubObjectivesCompletedDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Add objective to exit level
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            carExitInteractable.SetEnabled(true);
            carExitInteractable.onInteract.AddListener(OnInteractWithCarExit);
        }
        
        private void OnInteractWithCarExit()
        {
            // Remove previous listener
            carExitInteractable.onInteract.RemoveListener(OnInteractWithCarExit);

            // Show dialogue
            DialogueManager.Instance.SetDialogue(onExitLevelDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);

            // Level complete logic can be added here
            DialogueManager.Instance.onDialogueComplete.AddListener(OnLevelComplete);
        }

        private void OnLevelComplete()
        {
            // Remove previous listener
            DialogueManager.Instance.onDialogueComplete.RemoveListener(OnLevelComplete);
            
            EventsManager.OnGameCompleted?.Invoke();
        }
    }
}
