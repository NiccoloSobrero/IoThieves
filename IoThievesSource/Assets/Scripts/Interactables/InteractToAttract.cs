using Enemy;
using Levels;
using UnityEngine;

namespace Interactables
{
    public class InteractToAttract : MonoBehaviour, IInteractable
    {
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float attractionRange;
        
        private const int MaxAttractableEnemies = 10;
        private readonly Collider[] _enemies = new Collider[MaxAttractableEnemies];

        #region Tutorial Objectives

        private TutorialObjective _onInteract;

        #endregion
        
        public void Interact()
        {
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

            if (_onInteract != null)
            {
                _onInteract.Complete();
                _onInteract = null;
            }
        }

        public void SetOnInteractObjective(TutorialObjective onInteract)
        {
            _onInteract = onInteract;
        }
    }
}
