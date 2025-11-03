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

        private bool _isAlerted;
        private float _alertLevel;
        private float _alertIncrease;

        private void Update()
        {
            UpdateDetectionSphere();

            ManageAlertLevel();
        }

        private void UpdateDetectionSphere()
        {
            sphere.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void ManageAlertLevel()
        {
            if (_isAlerted)
            {
                _alertLevel += _alertIncrease * Time.deltaTime;
                        
                Debug.Log($"Alert: {_alertLevel} increased by {_alertIncrease}");
            }
            else if (_alertLevel > 0)
            {
                _alertLevel -= Time.deltaTime;

                if (_alertLevel < 0)
                {
                    _alertLevel = 0;
                }
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
            }
        }

        private void CheckForPlayerInRange(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Vector3 raycastDirection = (Utility.ZeroOutY(other.transform.position) - Utility.ZeroOutY(rayOrigin.position)).normalized;
                
                if (Physics.Raycast(rayOrigin.position,
                        raycastDirection,
                        out _,
                        detectionRange,
                        playerLayer))
                {
                    Debug.Log("Raycast hit");
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
            }
        }
    }
}
