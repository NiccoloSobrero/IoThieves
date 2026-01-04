using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class InteractableTriggerArea : MonoBehaviour
    {
        [SerializeField] private InteractableType interactableType;
        private bool IsObjective => interactableType is 
            InteractableType.HackableObjective or InteractableType.InteractableObjective;

        public bool IsPlayerInRange { get; private set; }
        public GameObject PlayerObject { get; private set; }
        
        private Interactable _interactable;

        private bool _isLocked;

        public void Init(Interactable interactable)
        {
            _interactable = interactable;

            _isLocked = false;
            
            ManualUpdate();
        }

        public void Lock(bool isLocked)
        {
            _isLocked = isLocked;
            
            ManualUpdate();
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsPlayerInRange = true;
                PlayerObject = other.gameObject;

                if (!_isLocked)
                {
                    _interactable.SetOutlineColor(interactableType, IsObjective);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsPlayerInRange = false;
                PlayerObject = null;

                if (!_isLocked)
                {
                    _interactable.SetOutlineColor(InteractableType.NotInteractable, IsObjective);
                }
            }
        }

        public void ManualUpdate()
        {
            if (_isLocked) return;
            
            _interactable.SetOutlineColor(IsPlayerInRange ? interactableType : InteractableType.NotInteractable, IsObjective);
        }
    }
}
