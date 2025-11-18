using Unity.Mathematics.Geometry;
using UnityEngine;

namespace AI
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float angularSpeed = .4f;
        //[SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private bool isMoving = true;
        
        [SerializeField] private Waypoint target;
        [SerializeField] private float threshold = .6f;
        [SerializeField] private float angularThreshold = 30.0f; //degrees

        void Update()
        {
            if (target is null)
                return;
            
            Vector3 dir = target.transform.position - transform.position;
            float distance = dir.magnitude;
            dir.Normalize();

            Vector3 forwardDir = transform.forward.normalized;
            
            float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
            float angleMag = Mathf.Abs(angle);
            
            if (angleMag < angularThreshold) isMoving = true;
            else isMoving = false;

            float rotateOf = angle * angularSpeed * Time.deltaTime;
            transform.Rotate(0,rotateOf,0);
            
            if (!isMoving)
                return;
            
            Vector3 velocity = dir * (speed * Time.deltaTime);
            transform.position  += new Vector3(velocity.x, 0, velocity.z);

            if (distance < threshold)
                target = target.GetNextPatrolTarget();
        }
    }
}
