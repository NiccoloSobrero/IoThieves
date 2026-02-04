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
        
        private Transform playerTransform;
        [SerializeField] private bool _playerInRange = false;
        [SerializeField] private bool _spotted = false;
        private float _timer = 0;
        private float _checkTime = 0.1f;
        
        private void Awake()
        {
            _timer = 0.0f;
            _playerInRange = false;
            SetDetectionSphere();
        }
        
        private void Update()
        {
            if (_playerInRange && !_spotted)
            {
                //print("Player in range");
                _timer += Time.deltaTime;
                
                if (_timer >= _checkTime)
                {
                    _timer = 0f;
                    CheckForPlayerInRange();
                }
            }
        }

        private void SetDetectionSphere()
        {
            transform.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                _playerInRange = true;
                playerTransform = other.gameObject.transform;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
                playerTransform = null;
                _spotted = false;
                alertManager.SetAlertedState(false);
            }
        }
        
        private void CheckForPlayerInRange()
        {
            Vector3 direction = playerTransform.position - transform.position;
            if (!Physics.Raycast(raycastOrigin.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore ))
            {
                _spotted = true;
                alertManager.SetAlertedState(true);
            }
        }

        
    }
}
