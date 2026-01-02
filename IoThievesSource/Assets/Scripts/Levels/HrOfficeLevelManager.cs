using Interactables;
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
        [SerializeField] private LevelObjectiveScrObj unlockBossDoor;
        [SerializeField] private LevelObjectiveScrObj getBossAddress;
        [SerializeField] private LevelObjectiveScrObj fileComplaint;
        [SerializeField] private LevelObjectiveScrObj getOut;

        [Header("Unlock door")]
        [SerializeField] private InteractToInvokeEvent keyInteractable;
        [SerializeField] private DoorTrigger bossDoorTrigger;
        
        [Header("Get address")]
        [SerializeField] private DesktopMinigame bossAddressMinigame;
        [SerializeField] private MinigameScrObj bossDesktopNavigationMinigame;
        
        [Header("File complaint")]
        [SerializeField] private TypeWriteMinigame typeWriteMinigame;

        [Header("Get out")] 
        [SerializeField] private LevelTrigger endLevelTrigger;

        private bool _completedFileComplaint;
        private bool _obtainedBossAddress;
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            // Initialization
            endLevelTrigger.gameObject.SetActive(false);

            _completedFileComplaint = false;
            _obtainedBossAddress = false;
            
            // First objectives set up
            bossDoorTrigger.SetEnabled(false);
            keyInteractable.SetEnabled(true);
            
            // Add listener for picking up the key
            keyInteractable.onInteract.AddListener(OnKeyInteract);
            // Add listener for writing the correct complaint
            typeWriteMinigame.onSendComplaint.AddListener(OnComplaintSent);
            
            levelObjectivesUI.AddObjective(unlockBossDoor);
            levelObjectivesUI.AddObjective(fileComplaint);
        }

        private void OnKeyInteract()
        {
            levelObjectivesUI.RemoveObjective(unlockBossDoor);
            
            keyInteractable.SetEnabled(false);
            keyInteractable.onInteract.RemoveListener(OnKeyInteract);
            
            bossDoorTrigger.SetEnabled(true);
            
            // Enable next objective
            levelObjectivesUI.AddObjective(getBossAddress);
            
            DownloadsManager.Instance.onDownloadTargetFile.AddListener(OnAddressDownloaded);
        }

        private void OnComplaintSent()
        {
            typeWriteMinigame.onSendComplaint.RemoveListener(OnComplaintSent);
            
            levelObjectivesUI.RemoveObjective(fileComplaint);
            _completedFileComplaint = true;

            if (_obtainedBossAddress)
            {
                SetUpLastStep();
            }
        }

        private void OnAddressDownloaded()
        {
            levelObjectivesUI.RemoveObjective(getBossAddress);
            
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
        }

        private void OnGetOut()
        {
            levelObjectivesUI.RemoveObjective(getOut);
            endLevelTrigger.onTriggerEnter.RemoveListener(OnGetOut);
            
            // End level
            
        }
    }
}
