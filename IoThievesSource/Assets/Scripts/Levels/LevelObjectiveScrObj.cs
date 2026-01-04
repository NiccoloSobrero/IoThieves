using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelObjective", menuName = "Levels/Level Objective")]
    public class LevelObjectiveScrObj : ScriptableObject
    {
        [TextArea(3, 5)]
        public List<string> objectiveSteps = new();

        private int _currentStep;

        public string Init()
        {
            _currentStep = 0;
            return objectiveSteps[_currentStep];
        }
        
        public string NextStep(out bool noMoreSteps)
        {
            if (_currentStep + 1 < objectiveSteps.Count)
            {
                _currentStep++;
                noMoreSteps = false;
                return objectiveSteps[_currentStep];
            }
            
            noMoreSteps = true;
            return objectiveSteps[_currentStep];
        }
    }
}
