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
        
        private Transform _playerTransform;
        [SerializeField] private bool _playerInRange = false;
        [SerializeField] private bool _spotted = false;
        private float _timer = 0;
        private float _checkTime = 0.1f;

        public void Initialize()
        {
            _timer = 0.0f;
            _playerInRange = false;
            _spotted = false;
            _playerTransform = null;
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
                _playerTransform = other.gameObject.transform;
            }
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
                _playerInRange = false;
                _playerTransform = null;
                _spotted = false;
                alertManager.SetAlertedState(false);
            }
        }
        
        private void CheckForPlayerInRange()
        {
            Vector3 direction = _playerTransform.position - transform.position;
            if (!Physics.Raycast(raycastOrigin.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore ))
            {
                _spotted = true;
                alertManager.SetAlertedState(true);
            }
        }

        
    }
}
