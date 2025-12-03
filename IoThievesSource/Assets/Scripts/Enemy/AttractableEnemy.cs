using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class AttractableEnemy : MonoBehaviour
    {
        
        [SerializeField] private float speed;

        private const float DistanceThreshold = 0.5f;
        private const float TurnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;
        
        public void MoveTowards(Vector3 targetPosition)
        {
            targetPosition.y = 0f;
            
            StartCoroutine(MoveTowardsRoutine(targetPosition));
        }

        private IEnumerator MoveTowardsRoutine(Vector3 targetPosition)
        {
            while (Vector3.Distance(targetPosition, transform.position) > DistanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
                
                Vector3 moveDir = (targetPosition - transform.position).normalized;
            
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                yield return null;
            }
        }
    }
}