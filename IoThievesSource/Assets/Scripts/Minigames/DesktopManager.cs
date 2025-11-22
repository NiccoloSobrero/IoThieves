using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigames
{
    public class DesktopManager : MonoBehaviour
    {
        public static DesktopManager Instance;
        
        [SerializeField] private GameObject desktopObject;
        [SerializeField] private Transform desktopParent;
        
        [SerializeField] private GameObject folderPrefab;
        
        [SerializeField] private GameObject folderObject;
        [SerializeField] private Transform folderParent;
        
        [SerializeField] private FolderContentScrObj folderContent;
        
        [SerializeField] private GameObject folderIsEmptyObject;
        
        [SerializeField] private TextMeshProUGUI pathText;
        private RectTransform _pathTextRect;
        
        private readonly List<string> _openPath = new();
        private readonly List<FolderContentScrObj> _pathScrObjs = new();
        
        private FolderContentScrObj _desktop;

        // private float _textPadding = 5f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _pathTextRect = pathText.gameObject.GetComponent<RectTransform>();
            
            OpenDesktop(folderContent);
        }
        
        private void GenerateFolder(FolderContentScrObj scrObj, Transform parent)
        {
            if (scrObj == null)
            {
                folderIsEmptyObject.SetActive(true);
                return;
            }
            
            foreach (FolderItem item in scrObj.folderItems)
            {
                Folder folder = Instantiate(folderPrefab, parent).GetComponent<Folder>();
                folder.Customize(item);
            }
        }

        public void OpenDesktop(FolderContentScrObj scrObj)
        {
            desktopObject.SetActive(true);
            GenerateFolder(scrObj, desktopParent);
            
            AddToPath("Desktop", scrObj);
            _desktop = scrObj;
            
            UpdatePathText();
        }

        public void OpenDesktopFolder(FolderContentScrObj scrObj, string folderName)
        {
            ClearChildren(folderParent);
            
            folderObject.SetActive(true);
            
            GenerateFolder(scrObj, folderParent);
            
            AddToPath(folderName, scrObj);
            
            UpdatePathText();
        }
        
        public void OpenFolder(FolderContentScrObj scrObj, string folderName)
        {
            ClearChildren(folderParent.transform);
            
            GenerateFolder(scrObj, folderParent);
            
            AddToPath(folderName, scrObj);
            
            UpdatePathText();
        }

        public void OpenFile()
        {
            Debug.Log("Do you want to download it?");
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
            string newPath = "";
            
            foreach (string folderName in _openPath)
            {
                newPath += folderName + "\\";
            }
            
            pathText.text = newPath;
            
            CheckTruncation(newPath);
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
            
            ClearChildren(folderParent.transform);

            _pathScrObjs.RemoveAt(_pathScrObjs.Count - 1);
            _openPath.RemoveAt(_openPath.Count - 1);
            
            GenerateFolder(_pathScrObjs[^1], folderParent);
            
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
            
            _openPath.Add("Desktop");
            _pathScrObjs.Add(_desktop);
        }
    }
}
