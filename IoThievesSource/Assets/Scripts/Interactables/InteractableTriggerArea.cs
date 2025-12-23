using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class InteractableTriggerArea : MonoBehaviour
    {
        public bool IsPlayerInRange { get; private set; }
        public GameObject PlayerObject { get; private set; }
        
        private Interactable _interactable;

        private bool _isLocked;

        public void Init(Interactable interactable)
        {
            _interactable = interactable;

            _isLocked = false;
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
                    _interactable.SetOutlineActive(true);
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
                    _interactable.SetOutlineActive(false);
                }
            }
        }

        private void ManualUpdate()
        {
            _interactable.SetOutlineActive(IsPlayerInRange);
        }
    }
}
