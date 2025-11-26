using Unity.Mathematics.Geometry;
using UnityEngine;

namespace AI
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float angularSpeed = .4f;
        [SerializeField] private bool isMoving = true;
        [SerializeField] private bool isInteracting = false;
        
        [SerializeField] private Waypoint target;
        [SerializeField] private float threshold; 
        [SerializeField] private float angularThreshold = 30.0f; //degrees

        void Update()
        {
            if (target is null)
                return;
            
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();

            Vector3 forwardDir = transform.forward.normalized;
            
            float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
            float angleMag = Mathf.Abs(angle);
            
            isMoving = angleMag < angularThreshold;

            float rotateOf = angle * angularSpeed * Time.deltaTime;
            transform.Rotate(0,rotateOf,0);

            if (isMoving)
            {
                Vector3 velocity = dir * (speed * Time.deltaTime);
                transform.position  += new Vector3(velocity.x, 0, velocity.z);
            }

            float distance = (target.transform.position - transform.position).magnitude;
            
            if (distance < threshold)
            {
                if (target is PointOfInterest poi)
                {
                    Debug.Log("THIS IS A POI");
                    isMoving = false;
                    isInteracting = !poi.interact();
                }

                if (!isInteracting)
                {
                    target = target.GetNextPatrolTarget();
                    isMoving = true;
                }
                
            }
        }
    }
}
