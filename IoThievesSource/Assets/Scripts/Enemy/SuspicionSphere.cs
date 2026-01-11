using AI;
using SuspicionSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Enemy
{
    public class SuspicionSphere : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AlertManager alertManager;
        
        [Header("Sphere")]
        [SerializeField] private float detectionRange;
        
        [SerializeField] private Transform raycastOrigin;

        
        private void Awake()
        {
            SetDetectionSphere();
        }

        private void SetDetectionSphere()
        {
            transform.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 direction = other.transform.position - transform.position;
            if (other.CompareTag("Player") && !Physics.Raycast(raycastOrigin.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore ))
            {
                alertManager.SetAlertedState(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                alertManager.SetAlertedState(false);
            }
        }
    }
}
