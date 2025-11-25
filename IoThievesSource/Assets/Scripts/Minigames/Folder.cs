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
        private float _value;
        private DesktopItemType _type;
        
        private FolderContentScrObj _contentScrObj;
        
        private DesktopMinigame _desktopMinigame;

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

        public void Customize(DesktopItem item, DesktopMinigame desktopMinigame)
        {
            icon.sprite = item.icon;
            label.text = item.itemName;
            _type = item.itemType;
            _value = item.value;

            _contentScrObj = item.folderContent;
            
            _desktopMinigame = desktopMinigame;

            if (item.itemType == DesktopItemType.TargetFile)
            {
                label.text = "<b><i><color=red>" + label.text;
            }
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
                _clickedOnce = false;
                
                switch (_type)
                {
                    case DesktopItemType.FolderOnDesktop:
                        _desktopMinigame.OpenDesktopFolder(_contentScrObj, label.text);
                        break;
                    case DesktopItemType.FolderInFolder:
                        _desktopMinigame.OpenFolder(_contentScrObj, label.text);
                        break;
                    case DesktopItemType.Program:
                        _desktopMinigame.OpenProgram();
                        break;
                    case DesktopItemType.File:
                        _desktopMinigame.OpenFile(label.text, _value);
                        break;
                    case DesktopItemType.TargetFile:
                        _desktopMinigame.OpenTargetFile(label.text, _value);
                        break;
                }
            }
        }
    }
}
