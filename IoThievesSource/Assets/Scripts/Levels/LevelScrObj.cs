using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelScrObj", menuName = "Levels/Level")]
    public class LevelScrObj : ScriptableObject
    {
        public List<LevelObjectiveScrObj> objectives = new();

        public void Register(LevelManager levelManager)
        {
            foreach (LevelObjectiveScrObj data in objectives)
            {
                data.Register(levelManager);
            }
        }

        public void Unregister(LevelManager levelManager)
        {
            foreach (LevelObjectiveScrObj data in objectives)
            {
                data.Unregister(levelManager);
            }
        }
    }
}
