using UnityEngine;
using UnityEngine.Events;

namespace Levels
{
    public class LevelTrigger : MonoBehaviour
    {
        [SerializeField] private string enteringTag;

        [HideInInspector] public UnityEvent onTriggerEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(enteringTag))
            {
                onTriggerEnter.Invoke();
            
                gameObject.SetActive(false);
            }
        }
    }
}