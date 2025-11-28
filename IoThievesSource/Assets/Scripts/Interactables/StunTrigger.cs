using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(Collider))]
    public class StunTrigger : MonoBehaviour
    {
        [SerializeField] private float range;
        
        private void Awake()
        {
            gameObject.transform.localScale = new Vector3(range, range, range);
            
            gameObject.SetActive(false);
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