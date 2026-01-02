using UnityEngine;
using UnityEngine.Events;

namespace Levels
{
    public class LevelTrigger : MonoBehaviour
    {
        private TutorialLevelManager _levelManager;

        [SerializeField] private string enteringTag;

        [HideInInspector] public UnityEvent onTriggerEnter;

        public void Init(TutorialLevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(enteringTag))
            {
                _levelManager.NextObjective();
                onTriggerEnter.Invoke();
            
                gameObject.SetActive(false);
            }
        }
    }
}
