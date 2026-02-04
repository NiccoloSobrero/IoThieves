using System.Collections;
using System.IO;
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
                //print("Player in range");
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
            detectionRange = enemyFov.Radius * 1.95f;
            transform.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") )
            {
                //alertManager.SetAlertedState(true);

                //print("Entered Player");
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
                //print("Exited Player");
                _playerInRange = false;
                _playerTransform = null; //************
            }
        }

        private void CheckForPlayerInRange()
        {
            if (Vector3.Angle(_playerTransform.position - transform.position, transform.forward) > enemyFov.Angle / 2)
            {
                return;
            }
            //print("Checking for PlayerInCone");

            // Compute direction to player
            Vector3 direction = _playerTransform.position - transform.position;
            float castDistance = direction.magnitude;
            direction.y = 0f;
            direction.Normalize();
             
            if (!Physics.Raycast(rayOrigin.position,
                    direction,
                    castDistance,
                    LayerMask.GetMask("Default") , QueryTriggerInteraction.Ignore))
            {
                print("DEVO TRIGGERARE GAME OVER");
                alertManager.ChangeState(alertManager.GameOverState);
            }
        }

        //-------------------------------------------------------

    }
}
