using Interactables.ThrowableDistraction;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToPickUp : Interactable
    {
        [SerializeField] private InteractableTriggerArea triggerArea;
        
        [Header("Throwable Settings")]
        [SerializeField] private GameObject distractionPrefab;
        [SerializeField] private float attractionRange;
        
        #region Events

        [HideInInspector] public UnityEvent onPickUpThrowable;

        #endregion

        private void Awake()
        {
            triggerArea.Init(this);
        }
        
        public override void Interact()
        {
            if (triggerArea.IsPlayerInRange && triggerArea.PlayerObject != null)
            {
                PlayerThrower player = triggerArea.PlayerObject.GetComponent<PlayerThrower>();

                if (player != null &&
                    player.TrySpawnAndPickUp(distractionPrefab, gameObject.transform.position, attractionRange))
                {
                    onPickUpThrowable?.Invoke();
                }
            }
        }
    }
}
