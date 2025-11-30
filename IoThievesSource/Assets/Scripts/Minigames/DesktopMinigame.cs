using System.Collections.Generic;
using Levels;
using TMPro;
using UnityEngine;

namespace Minigames
{
    public class DesktopMinigame : MonoBehaviour
    {
        private const string DesktopPath = "C:\\Desktop";
        
        [Header("References")]
        [SerializeField] private Transform desktopFoldersParent;
        
        [SerializeField] private GameObject folderPrefab;
        
        [SerializeField] private GameObject folderObject;
        [SerializeField] private Transform folderItemsParent;
        
        [SerializeField] private GameObject folderIsEmptyObject;
        
        [SerializeField] private TextMeshProUGUI pathText;
        private RectTransform _pathTextRect;
        
        private readonly List<string> _openPath = new();
        private readonly List<FolderContentScrObj> _pathScrObjs = new();
        
        private FolderContentScrObj _desktop;

        private bool _openedFirstFolder;

        [Header("Objective")]
        [SerializeField] private LevelObjectiveScrObj objective;
        
        private readonly List<string> _targetFiles = new();

        private void Awake()
        {
            _pathTextRect = pathText.gameObject.GetComponent<RectTransform>();

            _openedFirstFolder = false;
        }

        private void GenerateFolder(FolderContentScrObj scrObj, Transform parent)
        {
            ClearChildren(parent);
            
            if (scrObj == null)
            {
                folderIsEmptyObject.SetActive(true);
                return;
            }
            
            foreach (DesktopItem item in scrObj.folderItems)
            {
                Folder folder = Instantiate(folderPrefab, parent).GetComponent<Folder>();
                folder.Customize(item, this);
            }
        }

        public void InitMinigame(FolderContentScrObj desktopScrObj)
        {
            folderObject.SetActive(false);
            
            GenerateFolder(desktopScrObj, desktopFoldersParent);
            
            _desktop = desktopScrObj;
            
            _targetFiles.Clear();
            ComputeTargetFilesList(_desktop, DesktopPath + "\\");
            
            ResetPathToDesktop();
            
            UpdatePathText();
        }

        public void OpenDesktopFolder(FolderContentScrObj scrObj, string folderName)
        {
            if (_openedFirstFolder) return;
            
            folderObject.SetActive(true);
            
            GenerateFolder(scrObj, folderItemsParent);
            
            AddToPath(folderName, scrObj);
            
            UpdatePathText();
            
            _openedFirstFolder = true;
        }
        
        public void OpenFolder(FolderContentScrObj scrObj, string folderName)
        {
            GenerateFolder(scrObj, folderItemsParent);
            
            AddToPath(folderName, scrObj);
            
            UpdatePathText();
        }

        public void OpenFile(string fileName, float time)
        {
            string currentPath = GetCurrentPath();
            
            DownloadsManager.Instance.AddDownload(currentPath + fileName, time);
        }
        
        public void OpenTargetFile(string fileName, float time)
        {
            string currentPath = GetCurrentPath();
            
            DownloadsManager.Instance.AddDownload(currentPath + fileName, time);
        }
        
        public void OpenProgram()
        {
            Debug.Log("ERROR: can't open program");
        }

        private void ClearChildren(Transform parent)
        {
            folderIsEmptyObject.SetActive(false);
            
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

        private void UpdatePathText()
        {
            pathText.text = GetCurrentPath();
            
            CheckTruncation(pathText.text);
        }

        private void CheckTruncation(string textToTruncate)
        {
            pathText.ForceMeshUpdate();

            float availableWidth = _pathTextRect.rect.width;
            float preferredWidth = pathText.preferredWidth;

            // Fits fine → no truncation
            if (preferredWidth <= availableWidth)
                return;

            string ellipsis = "...";
            string text = textToTruncate;

            // Trim from the start until it fits (keep at least ellipsis)
            int startIndex = 0;
            int maxIterations = text.Length; // safety

            while (preferredWidth > availableWidth && startIndex < maxIterations)
            {
                startIndex += 3;
                string tail = text.Substring(startIndex);
                pathText.text = ellipsis + tail;
                pathText.ForceMeshUpdate();
                preferredWidth = pathText.preferredWidth;
            }
        }
        
        public void GoBack()
        {
            if (_pathScrObjs.Count <= 2) return;

            _pathScrObjs.RemoveAt(_pathScrObjs.Count - 1);
            _openPath.RemoveAt(_openPath.Count - 1);
            
            GenerateFolder(_pathScrObjs[^1], folderItemsParent);
            
            UpdatePathText();
        }

        private void AddToPath(string folderName, FolderContentScrObj scrObj)
        {
            _openPath.Add(folderName);
            _pathScrObjs.Add(scrObj);
        }

        public void ResetPathToDesktop()
        {
            _openPath.Clear();
            _pathScrObjs.Clear();
            
            _openPath.Add(DesktopPath);
            _pathScrObjs.Add(_desktop);

            _openedFirstFolder = false;
            
            ClearChildren(folderItemsParent);
        }

        private string GetCurrentPath()
        {
            string result = "";
            
            foreach (string s in _openPath)
            {
                result += s + "\\";
            }
            
            return result;
        }

        private void ComputeTargetFilesList(FolderContentScrObj folderContentScrObj, string currentPath)
        {
            foreach (DesktopItem folderItem in folderContentScrObj.folderItems)
            {
                if (folderItem.itemType == DesktopItemType.TargetFile)
                {
                    _targetFiles.Add(currentPath + folderItem.itemName);
                }
                else if (folderItem.itemType is DesktopItemType.FolderInFolder or DesktopItemType.FolderOnDesktop && 
                         folderItem.folderContent != null)
                {
                    ComputeTargetFilesList(folderItem.folderContent, currentPath + folderItem.itemName + "\\");
                }
            }
        }

        public void CheckCompletion()
        {
            List<string> downloadedFiles = DownloadsManager.Instance.DownloadedFiles;
            
            foreach (string targetFilePath in _targetFiles)
            {
                if (!downloadedFiles.Contains(targetFilePath))
                {
                    return;
                }
            }
            
            // Only execute if all target files are downloaded
            objective.Complete();
        }
    }
}
