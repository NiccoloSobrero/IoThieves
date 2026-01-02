using Interactables;
using Minigames;
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
        
        [Header("Get address")]
        [SerializeField] private DoorTrigger bossDoorTrigger;
        
        [Header("File complaint")]
        [SerializeField] private DesktopMinigame desktopMinigame;
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            bossDoorTrigger.SetEnabled(false);
            keyInteractable.SetEnabled(true);
            
            keyInteractable.onInteract.AddListener(OnKeyInteract);
            
            levelObjectivesUI.AddObjective(unlockBossDoor);
            levelObjectivesUI.AddObjective(fileComplaint);
        }

        private void OnKeyInteract()
        {
            keyInteractable.SetEnabled(false);
            keyInteractable.onInteract.RemoveListener(OnKeyInteract);
            
            bossDoorTrigger.SetEnabled(true);
            
            levelObjectivesUI.RemoveObjective(unlockBossDoor);
            
            levelObjectivesUI.AddObjective(getBossAddress);
        }

        private void OnComplaintSent()
        {
            levelObjectivesUI.RemoveObjective(fileComplaint);
        }

        private void OnAddressDownloaded()
        {
            levelObjectivesUI.RemoveObjective(getBossAddress);
            
            levelObjectivesUI.AddObjective(getOut);
        }

        private void OnGetOut()
        {
            levelObjectivesUI.RemoveObjective(getOut);
            
            // End level
            
        }
    }
}
