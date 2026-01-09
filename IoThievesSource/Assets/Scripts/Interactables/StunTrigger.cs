using System;
using AI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTrigger : MonoBehaviour
    {
        [SerializeField] private float ttl = 0.5f;
        private float elapsedTime = 0.0f;
        private void Awake()
        {
            float radius = 1f;

            // Get all colliders in the sphere
            Collider[] colliders = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);

            for (int i = 0 ; i<size; i++)
            {
                AIController ai = colliders[i].GetComponentInParent<AIController>();
                if (ai != null)
                {
                    Debug.Log("AIController found: " + colliders[i].name);
                    ai.ChangeState(AINames.StunnedState);
                }
            }
        }

        private void Update()
        {
           elapsedTime += Time.deltaTime;
           if (elapsedTime >= ttl)
               Destroy(gameObject);
        }

        public void Init(float radius)
        {
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
        }
        
        private void OnTriggerEnter(Collider other)
        {

            AIController enemy = other.gameObject.GetComponentInParent<AIController>();
             if (enemy is not null)
             {
                 enemy.ChangeState(AINames.StunnedState);
                 print("Destroying gameObject");
                 Destroy(gameObject);
             }
        }
    }
}