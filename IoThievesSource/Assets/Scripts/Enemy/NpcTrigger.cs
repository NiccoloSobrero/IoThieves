using System.Collections;
using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class NpcTrigger : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private AlertManager alertManager;
        [SerializeField] private EnemyFOVMesh enemyFov;

        [Header("Sphere")] 
        [SerializeField] private float detectionRange;

        [Header("Player check")] 
        [SerializeField] [Min(0.1f)] private float timeBetweenChecks = 0.1f;
        [SerializeField] private Transform rayOrigin;
        
        private Transform _playerTransform;
        private bool _playerInRange;
        private float _timer;

        private void Awake()
        {
            SetDetectionSphere();
        }

        private void Update()
        {
            if (_playerInRange)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= timeBetweenChecks)
                {
                    _timer = 0f;
                    CheckForPlayerInRange();
                }
            }
        }

        private void SetDetectionSphere()
        {
            detectionRange = enemyFov.Radius * 2.6f;
            transform.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") )
            {
                //alertManager.SetAlertedState(true);

                _playerTransform = other.transform;
                _playerInRange = true;
                _timer = 0f;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
               // alertManager.SetAlertedState(false);
                
                _playerInRange = false;
            }
        }

        private void CheckForPlayerInRange()
        {
            if (Vector3.Angle(_playerTransform.position - transform.position, transform.forward) > enemyFov.Angle / 2)
            {
                return;
            }

            // Compute direction to player
            Vector3 direction = _playerTransform.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
            
            if (!Physics.Raycast(rayOrigin.position,
                    direction,
                    enemyFov.Radius,
                    LayerMask.GetMask("Default") , QueryTriggerInteraction.Ignore))
            {
                alertManager.ChangeState(alertManager.GameOverState);
            }
        }

        //-------------------------------------------------------

        private IEnumerator ShrinkFov()
        {
            float duration = 0.15f;
            float elapsed = 0f;

            Vector3 startScale = transform.localScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

                yield return null;
            }

            // Ensure exact final value
            transform.localScale = Vector3.zero;
        }

        
        private IEnumerator EnlargeFov()
        {
            float duration = 0.15f;
            float elapsed = 0f;

            Vector3 startScale = transform.localScale;
            Vector3 endScale = new Vector3(detectionRange,detectionRange,detectionRange);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                transform.localScale = Vector3.Lerp(startScale,endScale, t);

                yield return null;
            }

            // Ensure exact final value
            transform.localScale = endScale;
            
        }
        
    }
}
