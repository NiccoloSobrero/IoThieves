using System.Collections.Generic;
using Levels;
using Managers;
using Minigames.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToMinigames : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<MinigameAndObjective> minigames = new();
        [SerializeField] private bool redoCompletedMinigameAfterReopen;

        [SerializeField] private float maxDistanceToInteract;
        [SerializeField] private LayerMask playerLayer;
        
        private readonly Collider[] _playerColliders = new Collider[1];

        #region Events
        
        [HideInInspector] public UnityEvent onInteract;

        #endregion
        
        public void Interact()
        {
            if (Physics.OverlapSphereNonAlloc(
                    gameObject.transform.position, 
                    maxDistanceToInteract, 
                    _playerColliders, 
                    playerLayer) > 0)
            {
                MinigamesManager.Instance.SetMinigamesLoop(this, minigames);

                onInteract?.Invoke();
            }
        }
        
        public void TryRemovingMinigame(MinigameAndObjective minigameAndObjective)
        {
            if (redoCompletedMinigameAfterReopen) return;
            
            minigames.Remove(minigameAndObjective);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, maxDistanceToInteract);
        }
    }

    [System.Serializable]
    public class MinigameAndObjective
    {
        public MinigameScrObj minigameScrObj;
        public LevelObjectiveScrObj objectiveScrObj; // Can be null
    }
}