using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToInvokeEvent : Interactable
    {
        [SerializeField] private InteractableTriggerArea triggerArea;
        [SerializeField] private bool disappearOnInteract;
        
        [HideInInspector] public UnityEvent onInteract;
        
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
                
                if (disappearOnInteract)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
            
            outlineRenderer.gameObject.SetActive(isEnabled);
        }
    }
}
