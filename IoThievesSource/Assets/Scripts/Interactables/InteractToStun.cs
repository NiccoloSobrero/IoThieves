using UnityEngine;

namespace Interactables
{
    public class InteractToStun : MonoBehaviour, IInteractable
    {
        [SerializeField] private StunTrigger stunTrigger;
        [SerializeField] private float stunRadius;

        private void Awake()
        {
            stunTrigger.Init(stunRadius);
        }
        
        public void Interact()
        {
            stunTrigger.gameObject.SetActive(true);
        }
    }
}