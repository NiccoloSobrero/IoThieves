using Levels;
using Managers;
using UnityEngine;

namespace Minigames.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DesktopMinigame", menuName = "Minigames/Desktop minigame")]
    public class DesktopMinigameScrObj : MinigameScrObj
    {
        public FolderContentScrObj folderContentScrObj;
        
        public override void InitMinigame(LevelObjectiveScrObj objectiveScrObj)
        {
            MinigamesManager.Instance.InitDesktopMinigame(folderContentScrObj, objectiveScrObj);
        }
    }
}