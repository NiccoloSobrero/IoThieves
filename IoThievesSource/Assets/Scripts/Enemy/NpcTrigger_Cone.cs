using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class NpcTrigger_Cone : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AlertManager alertManager;
        
        [Header("Sphere")]
        [SerializeField] private float scaleCone = 5.0f;

        private void Awake()
        {
            SetDetectionCone();
        }

        private void SetDetectionCone()
        {
            transform.localScale*=scaleCone; 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") 
                && IsVisible(other)) 
                
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

        private bool IsVisible(Collider other)
        {
            return !Physics.Raycast(transform.position, (other.transform.position - transform.position),
                Vector3.Distance(transform.position, other.transform.position), LayerMask.GetMask("Default"),QueryTriggerInteraction.Ignore);
        }
    }
}
