using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    [CreateAssetMenu(menuName = "FolderContent", fileName = "FolderContent")]
    public class FolderContentScrObj : ScriptableObject
    {
        public List<FolderItem> folderItems = new();
    }


    [Serializable]
    public class FolderItem
    {
        public string itemName;
        public FolderItemType itemType;
        public Sprite icon;
        public float value;

        public FolderContentScrObj folderContent;
    }


    public enum FolderItemType
    {
        FolderInFolder,
        FolderOnDesktop,
        File,
        Program
    }
}