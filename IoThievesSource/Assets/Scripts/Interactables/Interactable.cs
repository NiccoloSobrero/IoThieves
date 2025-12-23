using UnityEngine;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private GameObject outlineObject;

        public abstract void Interact();
        
        public void SetOutlineActive(bool active)
        {
            outlineObject.SetActive(active);
        }
    }
}