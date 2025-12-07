using Enemy;
using UnityEngine;

namespace Interactables.ThrowableDistraction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : MonoBehaviour
    {
        private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        [SerializeField] private LayerMask enemyLayer;
        private float _attractionRange;
        
        private const int MaxAttractableEnemies = 10;
        private readonly Collider[] _enemies = new Collider[MaxAttractableEnemies];

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            _rb.isKinematic = true;
        }

        public void Init(Transform parent, float attractionRange)
        {
            _attractionRange = attractionRange;
            
            _rb.isKinematic = true;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void OnThrown()
        {
            transform.SetParent(null);
            _rb.isKinematic = false;
        }

        private void OnCollisionEnter(Collision _)
        {
            HandleCollision();
        }

        protected virtual void HandleCollision()
        {
            Vector3 collisionPos = gameObject.transform.position;
            
            int size = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _attractionRange, _enemies, enemyLayer);

            for (int i = 0; i < size; i++)
            {
                if (_enemies[i].TryGetComponent(out AttractableEnemy enemy))
                {
                    enemy.MoveTowards(collisionPos);
                }
            }
            
            Destroy(gameObject);
        }
    }
}