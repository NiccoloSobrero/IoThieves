using UnityEngine;

namespace Levels
{
    public class LevelTrigger : MonoBehaviour
    {
        private TutorialLevelManager _levelManager;

        [SerializeField] private string enteringTag;

        public void Init(TutorialLevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(enteringTag))
            {
                _levelManager.NextObjective();
            
                gameObject.SetActive(false);
            }
        }
    }
}
