using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    [CreateAssetMenu(menuName = "FolderContent", fileName = "FolderContent")]
    public class FolderContentScrObj : ScriptableObject
    {
        public List<DesktopItem> folderItems = new();
    }


    [Serializable]
    public class DesktopItem
    {
        public string itemName;
        public DesktopItemType itemType;
        public Sprite icon;
        public float value;
        public Sprite documentSprite;

        public FolderContentScrObj folderContent;
    }


    public enum DesktopItemType
    {
        FolderInFolder,
        FolderOnDesktop,
        File,
        TargetFile,
        Program,
        HrComplaint
    }
}