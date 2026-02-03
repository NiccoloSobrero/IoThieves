using Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractToAttract : Interactable
    {
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float attractionRange;
        
        [SerializeField] private InteractableTriggerArea triggerArea;
        
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioSfx; 
        
        
        private const int MaxAttractableEnemies = 10;
        private readonly Collider[] _enemies = new Collider[MaxAttractableEnemies];

        #region Events

        [HideInInspector] public UnityEvent onInteract;

        #endregion

        private void Awake()
        {
            triggerArea.Init(this);
        }

        public override void Interact()
        {
            if (!triggerArea.IsPlayerInRange) return;
            
            if (audioSource != null && audioSfx != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioSfx;
                    audioSource.Play();
                }
            }
            
            int size = Physics.OverlapSphereNonAlloc(gameObject.transform.position, attractionRange, _enemies, enemyLayer);
            Vector3 attractPos = transform.position;
            attractPos.y = 0;

            for (int i = 0; i < size; i++)
            {
                if (_enemies[i].TryGetComponent(out AttractableEnemy enemy))
                {
                    enemy.MoveTowards(attractPos);
                }
            }

            onInteract?.Invoke();
        }
    }
}
