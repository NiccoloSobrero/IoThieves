using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class NpcTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AlertManager alertManager;
        
        [Header("Sphere")]
        [SerializeField] private float detectionRange;

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
            if (other.CompareTag("Player"))
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
