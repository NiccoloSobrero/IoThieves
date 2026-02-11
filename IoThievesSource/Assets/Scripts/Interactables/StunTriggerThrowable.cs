using AI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTriggerThrowable : StunTrigger
    {
        [SerializeField] private float ttl = 0.5f;
        [SerializeField] private float radius;
        
        private void Awake()
        {
            _elapsed = 0f;
            ScanArea();
        }

        private void Update()
        {
           _elapsed += Time.deltaTime;
           if (_elapsed >= ttl)
               Destroy(gameObject);
        }
        
        private void ScanArea()
        {
            Collider[] colliders = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, affectedLayer.value, QueryTriggerInteraction.Ignore);

            print("I hit " +size + " enemies " );
            for (int i = 0 ; i<size; i++)
            {
                AIController ai = colliders[i].GetComponentInParent<AIController>();
                if (ai is not null)
                {
                    Debug.Log("AIController found: " + colliders[i].name);
                    ai.ChangeState(AINames.StunnedState);
                }
            }
        }
    }
}