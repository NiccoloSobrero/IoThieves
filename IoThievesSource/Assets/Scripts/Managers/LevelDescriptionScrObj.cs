using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "Level Description", menuName = "LevelDescription")]
    public class LevelDescriptionScrObj : ScriptableObject
    {
        public Level levelNumber;
        public string levelName;
        public string levelDescription;
        public List<string> levelObjectives;
        [Range(0, 10)]
        public int difficulty;
    }
}
