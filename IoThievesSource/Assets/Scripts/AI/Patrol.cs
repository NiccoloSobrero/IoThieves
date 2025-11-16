using UnityEngine;

namespace AI
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;

        [SerializeField] private Waypoint target;
        [SerializeField] private float threshold = .6f;
        void Update()
        {
            if (target is null)
                return;
            
            Vector3 dir = target.transform.position - transform.position;
            float distance = dir.magnitude;
            dir.Normalize();
            
            Vector3 velocity = dir * (speed * Time.deltaTime);
            transform.position  += new Vector3(velocity.x, 0, velocity.z);

            if (distance < threshold)
                target = target.GetNextPatrolTarget();
        }
    }
}
