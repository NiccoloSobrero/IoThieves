using System;
using AI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTriggerThrowable : StunTrigger
    {
        [SerializeField] private float ttl = 0.5f;
        
        private void Awake()
        {
            _elapsed = 0f;
            radius = GetComponent<SphereCollider>().radius;
            ScanArea(radius);
        }

        private void Update()
        {
           _elapsed += Time.deltaTime;
           if (_elapsed >= ttl)
               Destroy(gameObject);
        }
    }
}