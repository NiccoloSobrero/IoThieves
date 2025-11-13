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
        
        private List<FolderContentScrObj> openPath = new();

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
            
            openPath.Add(scrObj);
            
            UpdatePathText();
        }

        public void OpenDesktopFolder(FolderContentScrObj scrObj)
        {
            ClearChildren(folderParent);
            
            folderObject.SetActive(true);
            GenerateFolder(scrObj, folderParent);
            
            openPath.Add(scrObj);
            
            UpdatePathText();
        }
        
        public void OpenFolder(FolderContentScrObj scrObj)
        {
            ClearChildren(folderParent.transform);
            
            GenerateFolder(scrObj, folderParent);
            
            openPath.Add(scrObj);
            
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

        public void UpdatePathText()
        {
            string newPath = "Desktop\\";
            
            foreach (FolderContentScrObj folder in openPath)
            {
                newPath += folder.name + "\\";
            }
            
            pathText.text = newPath;
        }
        
        public void GoBack()
        {
            
        }
    }
}
