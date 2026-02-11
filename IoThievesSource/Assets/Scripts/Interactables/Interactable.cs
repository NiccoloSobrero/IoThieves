using UnityEngine;

namespace Interactables
{
    public enum InteractableType
    {
        NotInteractable,
        Interactable,
        Hackable,
        InteractableObjective,
        HackableObjective
    }
    
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer outlineRenderer;
        
        private readonly Color _notInteractableColor = new Color(1f, 0f, 0f, 1f);
        private readonly Color _interactableColor = new Color(0f, 0.7f, 1f, 1f);
        private readonly Color _hackableColor = new Color(0.3f, 0.8f, 0.3f, 1f);
        private readonly Color _objectiveColor = new Color(0.9f, 0.8f, 0.1f, 1f);

        public abstract void Interact();
        
        public void SetOutlineColor(InteractableType interactionType, bool isObjective = false)
        {
            Color color;

            switch (interactionType)
            {
                case InteractableType.NotInteractable:
                    color = isObjective ? _objectiveColor : _notInteractableColor;
                    break;
                case InteractableType.Interactable:
                    color = _interactableColor;
                    break;
                case InteractableType.Hackable:
                    color = _hackableColor;
                    break;
                case InteractableType.InteractableObjective:
                    color = _interactableColor;
                    break;
                case InteractableType.HackableObjective:
                    color = _hackableColor;
                    break;
                default:
                    color = _notInteractableColor;
                    break;
            }
            
            foreach (Material material in outlineRenderer.materials)
            {
                material.color = color;
            }
        }
    }
}