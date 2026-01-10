using System;
using AI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTrigger : MonoBehaviour
    {
        [SerializeField] private float samplingTime = 0.2f;
        [SerializeField] protected float elapsed = 0.0f;
        [SerializeField] protected float _radius;
        private void Awake()
        {
            elapsed = 0;
            _radius = GetComponent<Collider>().bounds.extents.magnitude;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= samplingTime)
            {
                elapsed = 0.0f;
                ScanArea(_radius);
            }
        }

        private void OnEnable()
        {
           print("AC enabled");
        }

        private void OnDisable()
        {
            print("AC disabled");
           elapsed = 0; 
        }

        protected void ScanArea(float radius)
        {
            Collider[] colliders = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);

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
        
       /* 
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
                 //Destroy(gameObject);
             }
        }*/
    }
}