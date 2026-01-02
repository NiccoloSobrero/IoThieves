using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToInvokeEvent : Interactable
    {
        [HideInInspector] public UnityEvent onInteract;
        [SerializeField] private InteractableTriggerArea triggerArea;
        
        private bool _isEnabled;
        
        private void Awake()
        {
            triggerArea.Init(this);
        }
        
        public override void Interact()
        {
            if (!_isEnabled) return;
            
            if (triggerArea.IsPlayerInRange && triggerArea.PlayerObject != null)
            {
                onInteract.Invoke();
            }
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }
    }
}
