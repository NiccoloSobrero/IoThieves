using System.Collections;
using DialogueSystem;
using Events;
using Interactables;
using Managers;
using UI;
using UnityEngine;

namespace Levels
{
    public class BossHouseLevelManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelObjectivesUI levelObjectivesUI;
        
        [Header("Objectives")]
        [SerializeField] private LevelObjectiveScrObj findChatLogs;
        
        [Header("Intro")]
        [SerializeField] private DialogueLinesScrObj introDialogue;
        
        [SerializeField] private Transform cameraFollowTransform;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform bossOfficeCameraTarget;
        [SerializeField] private AnimationCurve cameraMoveCurve;
        
        private const float CameraTimeToTarget = 1f;
        
        [Header("Other steps")]
        [SerializeField] private DoorTrigger bossRoomDoorTrigger;
        [SerializeField] private DialogueLinesScrObj onArriveAtDoorDialogue;
        
        [SerializeField] private InteractToInvokeEvent laxativesInteractable;
        [SerializeField] private DialogueLinesScrObj onFoundLaxativesDialogue;
        
        [SerializeField] private InteractToInvokeEvent cupInteractable;
        
        [SerializeField] private DialogueLinesScrObj onChatDownloadDialogue;
        
        [SerializeField] private DialogueLinesScrObj onExitLevelDialogue;

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
        }

        private void Start()
        {
            Intro();
        }

        private void Intro()
        {
            // Show dialogue
            DialogueManager.Instance.SetDialogue(introDialogue);
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
            // Remove previous listener
            bossRoomDoorTrigger.onTriggerEnterEvent.RemoveListener(OnEnterBossRoomDoorTrigger);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onArriveAtDoorDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Set up next step
            laxativesInteractable.SetEnabled(true);
            laxativesInteractable.onInteract.AddListener(OnInteractWithLaxatives);
        }
        
        private void OnInteractWithLaxatives()
        {
            // Remove previous listener
            laxativesInteractable.onInteract.RemoveListener(OnInteractWithLaxatives);
            
            // Show dialogue
            DialogueManager.Instance.SetDialogue(onFoundLaxativesDialogue);
            DialogueManager.Instance.OpenDialogueWindow();
            
            // Set up next step
            cupInteractable.SetEnabled(true);
            cupInteractable.onInteract.AddListener(OnInteractWithCup);
        }

        private void OnInteractWithCup()
        {
            // Remove previous listener
            cupInteractable.onInteract.RemoveListener(OnInteractWithCup);
            
            // Execute action
            
            
            // Set up next step
            
        }
    }
}
