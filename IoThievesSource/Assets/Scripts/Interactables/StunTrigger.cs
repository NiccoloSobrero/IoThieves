using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTrigger : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Init(float radius)
        {
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Entered Stun Trigger");
            
            // if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            // {
            //     // Call function for stunning on Enemy
            // }
        }
    }
}