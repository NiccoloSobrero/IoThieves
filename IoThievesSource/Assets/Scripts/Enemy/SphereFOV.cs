using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class SphereFOV : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AlertManager alertManager;
        
        [Header("Raycast settings")]
        [SerializeField] private Transform rayOrigin;
        [SerializeField] private float detectionRange;
        [SerializeField, Range(0, 180)] private float fieldOfView;
        [SerializeField] private LayerMask playerLayer;
        
        [Header("Sphere")]
        [SerializeField] private Transform sphere;

        private bool _isAlerted;

        private void Awake()
        {
            SetDetectionSphere();
        }

        private void SetDetectionSphere()
        {
            sphere.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            alertManager.SetAlertedState(true);
        }

        private void OnTriggerExit(Collider other)
        {
            alertManager.SetAlertedState(false);
        }
    }
}
