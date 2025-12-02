using UnityEngine;

namespace Levels
{
    public class LevelTrigger : MonoBehaviour
    {
        private TutorialLevelManager _levelManager;

        public void Init(TutorialLevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            _levelManager.NextObjective();
            
            gameObject.SetActive(false);
        }
    }
}
