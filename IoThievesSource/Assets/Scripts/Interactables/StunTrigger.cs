using System;
using AI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTrigger : MonoBehaviour
    {
        [SerializeField] protected LayerMask affectedLayer;
        [SerializeField] private float samplingTime = 0.2f;
        protected float _elapsed;
        
        private BoxCollider _cubeColl;
        
        private Vector3 _cubeExtents;
        private Vector3 _cubePos;
        
        private void Awake()
        {
            _cubeColl =  GetComponent<BoxCollider>();
            _elapsed = 0;
            gameObject.SetActive(false);

            _cubeExtents = Vector3.Scale(_cubeColl.size * 0.5f, _cubeColl.transform.lossyScale);
            _cubePos = _cubeColl.transform.TransformPoint(_cubeColl.center);
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            
            if (_elapsed >= samplingTime)
            {
                _elapsed = 0.0f;
                ScanArea();
            }
        }

        private void OnDisable()
        {
           _elapsed = 0; 
        }

        private void ScanArea()
        {
            Collider[] colliders = new Collider[10];
            var size = Physics.OverlapBoxNonAlloc(_cubePos, _cubeExtents, colliders, _cubeColl.transform.rotation, affectedLayer.value, QueryTriggerInteraction.Ignore);

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

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireCube(transform.position + _cubePos, _cubeExtents);
        // }
    }
}