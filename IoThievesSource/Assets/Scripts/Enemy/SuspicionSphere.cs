using SuspicionSystem;
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

        public void Initialize()
        {
            SetDetectionSphere();
        }

        private void SetDetectionSphere()
        {
            transform.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }
        
        private void OnTriggerStay(Collider collision)
        {
            if (collision.CompareTag("Player"))
            {
                Vector3 dirToPlayer = collision.transform.position - transform.position;
                bool hasLineOfSight = !Physics.Raycast(
                    transform.position,
                    dirToPlayer.normalized,
                    dirToPlayer.magnitude,
                    LayerMask.GetMask("Default"),
                    QueryTriggerInteraction.Ignore
                );
        
                alertManager.SetAlertedState(hasLineOfSight);
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
