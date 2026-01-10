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
            elapsed = 0f;
            ScanArea(_radius);
        }

        private void Update()
        {
           elapsed += Time.deltaTime;
           if (elapsed >= ttl)
               Destroy(gameObject);
        }
    }
}