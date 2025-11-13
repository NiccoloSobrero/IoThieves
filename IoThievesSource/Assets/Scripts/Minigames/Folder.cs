using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    [RequireComponent(typeof(Button))]
    public class Folder : MonoBehaviour
    {
        private bool _clickedOnce;
        private float _doubleClickTimer;
        [SerializeField] private float deltaDoubleClick = 0.5f;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI label;
        private FolderItemType _type;
        
        private FolderContentScrObj _contentScrObj;


        private void Update()
        {
            if (_clickedOnce)
            {
                _doubleClickTimer -= Time.deltaTime;

                if (_doubleClickTimer <= 0)
                {
                    _clickedOnce = false;
                }
                
            }
        }


        public void Customize(FolderItem item)
        {
            icon.sprite = item.icon;
            label.text = item.itemName;
            _type = item.itemType;

            _contentScrObj = item.folderContent;
        }


        public void OnClick()
        {
            if (!_clickedOnce)
            {
                _clickedOnce = true;
                _doubleClickTimer = deltaDoubleClick;
            }
            else
            {
                Debug.Log("Double Click");
                _clickedOnce = false;
                
                switch (_type)
                {
                    case FolderItemType.FOLDER_ON_DESKTOP:
                        DesktopManager.Instance.OpenDesktopFolder(_contentScrObj);
                        break;
                    case FolderItemType.FOLDER_IN_FOLDER:
                        DesktopManager.Instance.OpenFolder(_contentScrObj);
                        break;
                    case FolderItemType.PROGRAM:
                        DesktopManager.Instance.OpenProgram();
                        break;
                    case FolderItemType.FILE: 
                        DesktopManager.Instance.OpenFile();
                        break;
                }
            }
            
            
            
            
            
        }
        
        
        
        
        
    }
}
