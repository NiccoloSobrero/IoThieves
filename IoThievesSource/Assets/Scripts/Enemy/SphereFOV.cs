using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class SphereFOV : MonoBehaviour
    {
        [Header("Raycast settings")]
        [SerializeField] private Transform rayOrigin;
        [SerializeField] private float detectionRange;
        [SerializeField, Range(0, 180)] private float fieldOfView;
        [SerializeField] private LayerMask playerLayer;
        
        [Header("Sphere")]
        [SerializeField] private Transform sphere;

        [Header("Billboard")]
        [SerializeField] private GameObject billboard;

        private bool _isAlerted;
        private float _alertIncrease;

        private void Update()
        {
            UpdateDetectionSphere();

            ManageAlertLevel();
            
            billboard.SetActive(_isAlerted);
        }

        private void UpdateDetectionSphere()
        {
            sphere.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void ManageAlertLevel()
        {
            if (_isAlerted)
            {
                SuspicionManager.Instance.IncreaseSuspicionLevel(_alertIncrease * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckForPlayerInRange(other);
        }

        private void OnTriggerStay(Collider other)
        {
            CheckForPlayerInRange(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isAlerted = false;
                SuspicionManager.Instance.RemoveEnemy(gameObject);
            }
        }

        private void CheckForPlayerInRange(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bool wasAlerted = _isAlerted;
                
                Vector3 raycastDirection = (Utility.ZeroOutY(other.transform.position) - Utility.ZeroOutY(rayOrigin.position)).normalized;
                
                if (Physics.Raycast(rayOrigin.position,
                        raycastDirection,
                        out _,
                        detectionRange,
                        playerLayer))
                {
                    if (Mathf.Abs(Vector3.Angle(gameObject.transform.forward, raycastDirection)) < fieldOfView / 2)
                    {
                        _alertIncrease = Vector3.Dot(gameObject.transform.forward, raycastDirection);
                        
                        _isAlerted = true;
                    }
                    else
                    {
                        _isAlerted = false;
                    }
                }
                else
                {
                    _isAlerted = false;
                }

                if (wasAlerted && !_isAlerted)
                {
                    SuspicionManager.Instance.RemoveEnemy(gameObject);
                }
                else if (!wasAlerted && _isAlerted)
                {
                    SuspicionManager.Instance.AddEnemy(gameObject);
                }
            }
        }
    }
}
