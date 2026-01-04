using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames
{
    public class DesktopMinigame : MonoBehaviour
    {
        private const string DesktopPath = "C:\\Desktop";

        [SerializeField] private Canvas canvas;

        [Header("Desktop")] 
        [SerializeField] private GameObject desktopObj;
        [SerializeField] private RectTransform desktopRect;
        [SerializeField] private Transform desktopFoldersParent;
        [SerializeField] private GameObject folderPrefab;
        
        private Vector2 _desktopLastPosition;
        private Vector2 _desktopLastSize;
        private bool _isDesktopMaximized;
        
        [Header("Folder")]
        [SerializeField] private GameObject folderObject;
        [SerializeField] private RectTransform folderRect;
        [SerializeField] private Transform folderItemsParent;
        [SerializeField] private RectTransform windowsToOpenRect; // Rect that contains all the windows that can be open in desktop
        
        [SerializeField] private GameObject folderIsEmptyObject;
        
        [SerializeField] private TextMeshProUGUI pathText;
        private RectTransform _pathTextRect;
        
        private Vector2 _folderLastPosition;
        private Vector2 _folderLastSize;
        private bool _isFolderMaximized;
        
        private bool _openedFirstFolder;
        [SerializeField] private GameObject toolbarFolderObject;

        [Header("Open File")]
        [SerializeField] private GameObject openFileObj;
        [SerializeField] private RectTransform openFileRect;
        [SerializeField] private DesktopNavFile desktopNavFile;
        
        private Vector2 _openFileLastPosition;
        private Vector2 _openFileLastSize;
        private bool _isOpenFileMaximized;
        
        private bool _openedTextFile;
        [SerializeField] private GameObject toolbarOpenItemObject;
        
        [Header("HR Complaint")]
        [SerializeField] private GameObject hrComplaintObj;
        [SerializeField] private RectTransform hrComplaintRect;
        [SerializeField] private TypeWriteMinigame hrComplaintMinigame;
        
        private Vector2 _hrComplaintLastPosition;
        private Vector2 _hrComplaintLastSize;
        private bool _isHrComplaintMaximized;
        
        private bool _openedHrComplaint;
        [SerializeField] private GameObject toolbarHrComplaintObject;
        
        [Header("Toolbar")]
        [SerializeField] private GameObject toolbarItemPrefab;
        [SerializeField] private Transform toolbarItemParent;
        
        private readonly List<string> _openPath = new();
        private readonly List<FolderContentScrObj> _pathScrObjs = new();

        private string _currentOpenFileName;
        private float _currentOpenFileDownloadTime;
        private bool _isCurrentOpenFileTarget;
        
        private FolderContentScrObj _desktop;
        
        private readonly List<string> _targetFiles = new();
        
        #region Events

        [HideInInspector] public UnityEvent onOpenFolderWithTarget;

        #endregion

        private void Awake()
        {
            _pathTextRect = pathText.gameObject.GetComponent<RectTransform>();

            _openedFirstFolder = false;
            _openedTextFile = false;

            // Initialize top bar handling variables
            _desktopLastPosition = desktopRect.rect.position;
            _desktopLastSize = desktopRect.rect.size;
            _isDesktopMaximized = false;
            
            _folderLastPosition = folderRect.rect.position;
            _folderLastSize = folderRect.rect.size;
            _isFolderMaximized = false;
            
            _openFileLastPosition = openFileRect.rect.position;
            _openFileLastSize = openFileRect.rect.size;
            _isOpenFileMaximized = false;
            
            _hrComplaintLastPosition = hrComplaintRect.rect.position;
            _hrComplaintLastSize = hrComplaintRect.rect.size;
            _isHrComplaintMaximized = false;
        }

        private void GenerateFolder(FolderContentScrObj scrObj, Transform parent)
        {
            ClearChildren(parent);
            
            if (scrObj == null)
            {
                folderIsEmptyObject.SetActive(true);
                return;
            }
            
            bool hasTargetFile = false;
            
            foreach (DesktopItem item in scrObj.folderItems)
            {
                Folder folder = Instantiate(folderPrefab, parent).GetComponent<Folder>();
                folder.Customize(item, this);

                if (item.itemType == DesktopItemType.TargetFile)
                {
                    hasTargetFile = true;
                }
            }

            if (hasTargetFile)
            {
                onOpenFolderWithTarget?.Invoke();
            }
        }

        public void InitMinigame(FolderContentScrObj desktopScrObj)
        {
            _desktop = desktopScrObj;
            
            folderObject.SetActive(false);
            openFileObj.SetActive(false);
            
            GenerateFolder(desktopScrObj, desktopFoldersParent);
            
            _targetFiles.Clear();
            ComputeTargetFilesList(_desktop, DesktopPath + "\\");
            
            ResetPathToDesktop();
            
            UpdatePathText();
        }

        public void OpenDesktopFolder(FolderContentScrObj scrObj, string folderName)
        {
            if (_openedFirstFolder) return;
            
            folderObject.SetActive(true);
            folderObject.transform.SetAsLastSibling();
            
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

        public void OpenFile(string fileName, float time, Sprite documentSprite)
        {
            string currentPath = GetCurrentPath();
            
            _currentOpenFileName = currentPath + fileName;
            _currentOpenFileDownloadTime = time;
            _isCurrentOpenFileTarget = false;
            
            desktopNavFile.AdaptToImage(documentSprite, fileName);
            
            // Set file graphics
            openFileObj.SetActive(true);
            openFileObj.transform.SetAsLastSibling();

            _openedTextFile = true;
        }
        
        public void OpenTargetFile(string fileName, float time, Sprite documentSprite)
        {
            string currentPath = GetCurrentPath();
            
            _currentOpenFileName = currentPath + fileName;
            _currentOpenFileDownloadTime = time;
            _isCurrentOpenFileTarget = true;
            
            desktopNavFile.AdaptToImage(documentSprite, fileName);
            
            // Set file graphics
            openFileObj.SetActive(true);
            openFileObj.transform.SetAsLastSibling();
            
            _openedTextFile = true;
        }

        public void DownloadFile()
        {
            if (_isCurrentOpenFileTarget)
            {
                DownloadsManager.Instance.AddDownload(_currentOpenFileName, _currentOpenFileDownloadTime, _isCurrentOpenFileTarget);
            }
            else
            {
                DownloadsManager.Instance.AddDownload(_currentOpenFileName, _currentOpenFileDownloadTime, _isCurrentOpenFileTarget);
            }
        }
        
        public void OpenProgram()
        {
            Debug.Log("ERROR: can't open program");
        }

        public void OpenComplaint()
        {
            hrComplaintMinigame.transform.SetAsLastSibling();
            hrComplaintMinigame.gameObject.SetActive(true);
            
            hrComplaintMinigame.Init();

            _openedHrComplaint = true;
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
            
            EventsManager.OnMinigameCompleted?.Invoke();
        }
        
        public void MinimizeWindow(GameObject window)
        {
            if (window == folderObject && _openedFirstFolder)
            {
                window.SetActive(false);
            }
            else if (window == openFileObj && _openedTextFile)
            {
                window.SetActive(false);
            }
            else if (window == hrComplaintObj && _openedHrComplaint)
            {
                window.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Not implemented");
            }
        }

        public void MaximizeOrRestoreWindow(GameObject window)
        {
            if (window == desktopObj)
            {
                if (_isDesktopMaximized)
                {
                    desktopRect.anchoredPosition = new Vector2(_desktopLastPosition.x, _desktopLastPosition.y);
                    desktopRect.sizeDelta = new Vector2(_desktopLastSize.x, _desktopLastSize.y);
                }
                else
                {
                    _desktopLastPosition = desktopRect.anchoredPosition;
                    _desktopLastSize = desktopRect.sizeDelta;
                        
                    desktopRect.anchoredPosition = Vector2.zero;
                    desktopRect.sizeDelta = canvas.renderingDisplaySize;
                }
                    
                _isDesktopMaximized = !_isDesktopMaximized;
            }
            else if (window == folderObject)
            {
                if (_isFolderMaximized)
                {
                    folderRect.anchoredPosition = new Vector2(_folderLastPosition.x, _folderLastPosition.y);
                    folderRect.sizeDelta = new Vector2(_folderLastSize.x, _folderLastSize.y);
                }
                else
                {
                    _folderLastPosition = folderRect.anchoredPosition;
                    _folderLastSize = folderRect.sizeDelta;
                        
                    folderRect.anchoredPosition = Vector2.zero;
                    folderRect.sizeDelta = windowsToOpenRect.rect.size;
                }
                
                _isFolderMaximized = !_isFolderMaximized;
            }
            else if (window == openFileObj)
            {
                if (_isOpenFileMaximized)
                {
                    openFileRect.anchoredPosition = new Vector2(_openFileLastPosition.x, _openFileLastPosition.y);
                    openFileRect.sizeDelta = new Vector2(_openFileLastSize.x, _openFileLastSize.y);
                }
                else
                {
                    _openFileLastPosition = openFileRect.anchoredPosition;
                    _openFileLastSize = openFileRect.sizeDelta;
                        
                    openFileRect.anchoredPosition = Vector2.zero;
                    openFileRect.sizeDelta = windowsToOpenRect.rect.size;
                }
                    
                _isOpenFileMaximized = !_isOpenFileMaximized;
            }
            else if (window == hrComplaintObj)
            {
                if (_isHrComplaintMaximized)
                {
                    hrComplaintRect.anchoredPosition = new Vector2(_hrComplaintLastPosition.x, _hrComplaintLastPosition.y);
                    hrComplaintRect.sizeDelta = new Vector2(_hrComplaintLastSize.x, _hrComplaintLastSize.y);
                }
                else
                {
                    _hrComplaintLastPosition = hrComplaintRect.anchoredPosition;
                    _hrComplaintLastSize = hrComplaintRect.sizeDelta;
                        
                    hrComplaintRect.anchoredPosition = Vector2.zero;
                    hrComplaintRect.sizeDelta = windowsToOpenRect.rect.size;
                }
                    
                _isHrComplaintMaximized = !_isHrComplaintMaximized;
            }
            else
            {
                Debug.LogWarning("Not implemented");
            }
        }

        public void PopWindowBackUp(GameObject window)
        {
            if (window == folderObject && _openedFirstFolder)
            {
                window.transform.SetAsLastSibling();
                window.SetActive(true);
            }
            else if (window == openFileObj && _openedTextFile)
            {
                window.transform.SetAsLastSibling();
                window.SetActive(true);
            }
            else if (window == hrComplaintObj && _openedHrComplaint)
            {
                window.transform.SetAsLastSibling();
                window.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Not implemented");
            }
        }

        public void CloseTextFile()
        {
            openFileObj.SetActive(false);
            _openedTextFile = false;
        }

        public void CloseHrComplaint()
        {
            hrComplaintObj.SetActive(false);
            _openedHrComplaint = false;
        }
    }
}