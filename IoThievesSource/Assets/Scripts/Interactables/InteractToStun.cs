using UnityEngine;

namespace Interactables
{
    public class InteractToStun : Interactable
    {
        [SerializeField] private StunTrigger stunTrigger;
        [SerializeField] private float stunRadius;
        
        [SerializeField] private InteractableTriggerArea triggerArea;

        private void Awake()
        {
            stunTrigger.Init(stunRadius);
            
            triggerArea.Init(this);
        }
        
        public override void Interact()
        {
            if (!triggerArea.IsPlayerInRange) return;
            
            stunTrigger.gameObject.SetActive(true);
        }
    }
}