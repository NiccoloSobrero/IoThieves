using UnityEngine;

namespace Interactables
{
    public class InteractToStun : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject stunSphere;
        
        public void Interact()
        {
            stunSphere.SetActive(true);
        }
    }
}