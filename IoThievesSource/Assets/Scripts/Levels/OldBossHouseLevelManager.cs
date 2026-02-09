using System.Collections;
using AI;
using AI.AI_States;
using AI.NavigationAndWaypoints;
using DialogueSystem;
using Events;
using Interactables;
using Managers;
using Minigames;
using UI;
using UnityEngine;

namespace Levels
{
    public class OldBossHouseLevelManger : LevelManager
    {
        [Header("References")]
        [SerializeField] private LevelObjectivesUI levelObjectivesUI;
        
        [Header("Objectives")]
        [SerializeField] private LevelObjectiveScrObj findChatLogs;
        
        [Header("Intro")]
        [SerializeField] private DialogueLinesScrObj introDialogue;
        
        [SerializeField] private Transform cameraFollowTransform;
        [SerializeField] private Transform bossOfficeCameraTarget;
        [SerializeField] private AnimationCurve cameraMoveCurve;
        
        private const float CameraTimeToTarget = 1f;
        
        [Header("Boss room door trigger")]
        [SerializeField] private DialogueLinesScrObj onArriveAtDoorDialogue;
        [SerializeField] private DoorTrigger bossRoomDoorTrigger;
        
        [Header("Find laxatives")]
        [SerializeField] private DialogueLinesScrObj onFoundLaxativesDialogue;
        [SerializeField] private InteractToInvokeEvent laxativesInteractable;
        
        [Header("Put laxatives in cup")]
        [SerializeField] private InteractToInvokeEvent cupInteractable;
        [SerializeField] private AIController bossAIController;
        [SerializeField] private Waypoint cupWaypoint;
        [SerializeField] private Waypoint toiletWaypoint;
        [SerializeField] private float bossTimeToDrink = 5f;
        
        [Header("Download chat logs")]
        [SerializeField] private DialogueLinesScrObj onChatDownloadDialogue;
        
        [Header("Exit level")]
        [SerializeField] private DialogueLinesScrObj onExitLevelDialogue;
        [SerializeField] private InteractToInvokeEvent carExitInteractable;
        
        /*
         * Flow:
         *      - Start from entrance of boss house, intro dialogue explaining
         *      - The door is closed, have to put laxative in boss's drink
         *      - Find laxatives in one of the bathrooms
         *      - Put laxatives in boss's drink
         *      - Attract boss to the cup
         *      - Boss feels sick, go to the bathroom (door unlocks)
         *      - Find boss's computer, download chat logs (The chats are in another device?)
         *      - Exit the level
         */
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            bossRoomDoorTrigger.SetLocked(true);
            
            laxativesInteractable.SetEnabled(false);
            
            cupInteractable.SetEnabled(false);
            
            carExitInteractable.SetEnabled(false);
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
            laxativesInteractable.SetEnabled(true);
            laxativesInteractable.onInteract.AddListener(OnInteractWithLaxatives);
        }
        
        private void OnInteractWithLaxatives()
        {
            SaveLastValidPosition();
            
            // Remove previous listener
            laxativesInteractable.onInteract.RemoveListener(OnInteractWithLaxatives);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onFoundLaxativesDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            // Set up next step
            cupInteractable.SetEnabled(true);
            cupInteractable.onInteract.AddListener(OnInteractWithCup);
        }

        private void OnInteractWithCup()
        {
            StartCoroutine(OnInteractWithCupCor());
        }
        
        private IEnumerator OnInteractWithCupCor()
        {
            // Remove previous listener
            cupInteractable.onInteract.RemoveListener(OnInteractWithCup);
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            // Execute action
            bossAIController.ChangeState(AINames.GoToDestination);

            if (bossAIController.CurrentState is GoToDestination bossStateGtd)
            {
                bossStateGtd.SetDestination(cupWaypoint);
            }
            
            // Wait for boss to reach cup
            yield return new WaitUntil(() => Vector3.Distance(bossAIController.transform.position, cupWaypoint.transform.position) < 1f);
            
            // Wait for the boss to drink
            bossAIController.ChangeState(AINames.Idle);
            yield return new WaitForSeconds(bossTimeToDrink);
            
            // Unlock boss room door
            bossRoomDoorTrigger.SetLocked(false);
            
            // Make boss go to toilet
            bossAIController.ChangeState(AINames.GoToDestination);
            bossStateGtd = bossAIController.CurrentState as GoToDestination;
            
            if (bossStateGtd != null)
            {
                bossStateGtd.SetDestination(toiletWaypoint);
            }
            
            // Wait for boss to reach toilet
            yield return new WaitUntil(() => Vector3.Distance(bossAIController.transform.position, toiletWaypoint.transform.position) < 1f);
            
            // Set boss stunned in toilet
            bossAIController.ChangeState(AINames.StunnedState);
            
            // Set up next step
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(OnAddressDownloaded);
        }
        
        private void OnAddressDownloaded()
        {
            SaveLastValidPosition();
            
            // Remove previous listener
            DownloadsManager.Instance.onDownloadTargetFile.RemoveListener(OnAddressDownloaded);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onChatDownloadDialogue, PlayerInputManager.Instance.CurrentContext);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Update objectives
            levelObjectivesUI.UpdateObjective(findChatLogs);
            
            // Set up next step
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
