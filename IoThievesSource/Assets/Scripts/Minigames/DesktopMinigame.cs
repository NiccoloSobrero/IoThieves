using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigames
{
    public class DesktopMinigame : MonoBehaviour
    {
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

        private void Awake()
        {
            _pathTextRect = pathText.gameObject.GetComponent<RectTransform>();

            _openedFirstFolder = false;
        }

        private void GenerateFolder(FolderContentScrObj scrObj, Transform parent)
        {
            if (scrObj == null)
            {
                folderIsEmptyObject.SetActive(true);
                return;
            }
            
            ClearChildren(parent);
            
            foreach (DesktopItem item in scrObj.folderItems)
            {
                Folder folder = Instantiate(folderPrefab, parent).GetComponent<Folder>();
                folder.Customize(item, this);
            }
        }

        public void InitMinigame(FolderContentScrObj scrObj)
        {
            folderObject.SetActive(false);
            
            GenerateFolder(scrObj, desktopFoldersParent);
            
            _desktop = scrObj;
            
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
            
            Debug.Log("Found target file.");
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
            
            _openPath.Add("C:\\Desktop");
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
    }
}
