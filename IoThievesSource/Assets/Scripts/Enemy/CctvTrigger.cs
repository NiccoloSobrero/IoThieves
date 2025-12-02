using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class CctvTrigger : MonoBehaviour
    {
        [SerializeField] private float radius;
        private const float CylinderHeight = 0.1f;

        private void Awake()
        {
            gameObject.transform.localScale = new Vector3(radius, CylinderHeight, radius);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Game Over.");
            }
        }
    }
}
