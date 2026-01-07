using TMPro;
using UnityEngine;

namespace Managers
{
    public class LevelSelectorManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelDescription;
        [SerializeField] private RectTransform contentRect;

        private const float YOffset = 50f;

        private const char FullProgressChar = '█';
        private const char EmptyProgressChar = '░';
        
        public LevelDescriptionScrObj levelScr;

        private void Start()
        {
            SetLevelDescription(levelScr);
        }

        private void SetLevelDescription(LevelDescriptionScrObj levelScrObj)
        {
            string description = "<color=green>> hack --target level_00</color>\n\n";
            description += "<b>TARGET_NAME</b>\n";
            description += $"   level_{levelScrObj.levelNumber:00} - {levelScrObj.levelName}\n\n";
            
            description += "<b>DESCRIPTION</b>\n";
            description += $"   {levelScrObj.levelDescription}\n\n";
            
            description += "<b>OBJECTIVES</b>\n";
            foreach (string levelObjective in levelScrObj.levelObjectives)
            {
                description += $"   --{levelObjective}\n";
            }
            description += "\n";
            
            description += "<b>DIFFICULTY</b>\n";
            description += $"   <color=#{ColorFromDifficulty(levelScrObj.difficulty)}>";
            for (int i = 0; i < levelScrObj.difficulty; i++)
            {
                description += FullProgressChar;
            }
            for (int i = levelScrObj.difficulty; i < 10; i++)
            {
                description += EmptyProgressChar;
            }
            description += "</color>\n\n";

            description += "> Press <b>ENTER</b> to play...";
            
            levelDescription.text = description;
            levelDescription.ForceMeshUpdate();
            
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, levelDescription.preferredHeight + YOffset);
        }

        private string ColorFromDifficulty(int difficulty)
        {
            difficulty = Mathf.Clamp(difficulty, 0, 10);

            if (difficulty <= 5)
            {
                byte r = (byte)Mathf.RoundToInt((difficulty / 5f) * 255f);
                return $"{r:X2}FF00";
            }

            byte g = (byte)Mathf.RoundToInt((1f - (difficulty - 5) / 5f) * 255f);
            return $"FF{g:X2}00";
        }
    }
}
