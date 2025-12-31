using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Minigames
{
    public class ButtonSelectionListener : MonoBehaviour, IDeselectHandler, ISelectHandler
    {
        public UnityAction<BaseEventData> OnDeselected;
        public UnityAction<BaseEventData> OnSelected;

        public void OnDeselect(BaseEventData eventData)
        {
            OnDeselected?.Invoke(eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelected?.Invoke(eventData);
        }
    }
}
