using AI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask affectedLayer;
        [SerializeField] private float samplingTime = 0.2f;
        protected float _elapsed = 0.0f;
        [SerializeField] protected float radius;
        
        private void Awake()
        {
            _elapsed = 0;
            radius = GetComponent<Collider>().bounds.extents.magnitude;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed >= samplingTime)
            {
                _elapsed = 0.0f;
                ScanArea(radius);
            }
        }

        private void OnEnable()
        {
           print("AC enabled");
        }

        private void OnDisable()
        {
            print("AC disabled");
           _elapsed = 0; 
        }

        protected void ScanArea(float scanRadius)
        {
            Collider[] colliders = new Collider[10];
            Debug.Log("SCAN AREA WITH RADIUS "+ radius+" I AM "+ LayerMask.LayerToName(affectedLayer.value));
            var size = Physics.OverlapSphereNonAlloc(transform.position, scanRadius, colliders, affectedLayer.value, QueryTriggerInteraction.Ignore);

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