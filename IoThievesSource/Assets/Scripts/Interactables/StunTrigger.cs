using AI;
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
             if (other.gameObject.TryGetComponent<AIController>(out var enemy))
             {
                 enemy.ChangeState(AINames.StunnedState);
                 // Call function for stunning on Enemy
             }
        }
    }
}