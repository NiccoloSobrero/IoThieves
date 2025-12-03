using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelObjective", menuName = "Levels/Level Objective")]
    public class LevelObjectiveScrObj : ScriptableObject
    {
        // Use these scriptable objects just like an event for an event system with SO
        // Plug the same SO in both the level manager and the interaction that completes the objective specified by the SO
        // I use only one level manager because I want the objective to be registered only on one level at a time (without using singletons)
        private LevelManager _manager;
        
        public string objectiveDescription;

        // Call from the minigame/interaction that completes the objective
        public void Complete()
        {
            _manager?.Complete(this);
        }

        public void Register(LevelManager manager)
        {
            _manager = manager;
        }

        public void Unregister(LevelManager manager)
        {
            if (_manager != manager)
            {
                return;
            }
            
            _manager = null;
        }
    }
}
