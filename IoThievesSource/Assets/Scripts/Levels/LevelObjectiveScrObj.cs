using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelObjective", menuName = "Levels/Level Objective")]
    public class LevelObjectiveScrObj : ScriptableObject
    {
        public string objectiveDescription;
    }
}
