using UnityEngine;

namespace Enemy
{
    public class EnemyFOVCustomizer : MonoBehaviour
    {
        [SerializeField] private Transform leftCube;
        [SerializeField] private Transform rightCube;

        [SerializeField, Range(90, 180)] private float fieldOfView;
        [SerializeField, Min(1)] private float range;

        private void Update()
        {
            UpdateFOV();
        }

        private void UpdateFOV()
        {
            leftCube.localScale = new Vector3(range, 1, range);
            rightCube.localScale = new Vector3(range, 1, range);

            leftCube.rotation = Quaternion.Euler(0, -fieldOfView / 2, 0);
            rightCube.rotation = Quaternion.Euler(0, fieldOfView / 2, 0);
            
            leftCube.localPosition = new Vector3(-Mathf.Cos((-fieldOfView / 2 + 135) * Mathf.Deg2Rad) * Mathf.Sqrt(2) * range / 2, 
                leftCube.localPosition.y, 
                Mathf.Sin((-fieldOfView / 2 + 135) * Mathf.Deg2Rad) * Mathf.Sqrt(2) * range / 2);
            rightCube.localPosition = new Vector3(Mathf.Cos((-fieldOfView / 2 + 135) * Mathf.Deg2Rad) * Mathf.Sqrt(2) * range / 2, 
                rightCube.localPosition.y, 
                Mathf.Sin((-fieldOfView / 2 + 135) * Mathf.Deg2Rad) * Mathf.Sqrt(2) * range / 2);
        }
    }
}
