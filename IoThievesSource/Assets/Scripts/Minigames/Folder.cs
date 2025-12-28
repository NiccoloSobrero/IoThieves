using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private Sprite _documentSprite;
        private float _value;
        private DesktopItemType _type;
        
        private FolderContentScrObj _contentScrObj;
        
        private DesktopMinigame _desktopMinigame;

        private string _fileName;
        
        [SerializeField] private ButtonSelectionListener buttonSelectionListener;

        private void OnEnable()
        {
            buttonSelectionListener.OnDeselected += OnDeselect;
            buttonSelectionListener.OnSelected += OnSelect;
        }

        private void OnDisable()
        {
            buttonSelectionListener.OnDeselected -= OnDeselect;
            buttonSelectionListener.OnSelected -= OnSelect;
        }

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
            _fileName = item.itemName;
            _type = item.itemType;
            _value = item.value;
            _documentSprite = item.documentSprite;
            
            _contentScrObj = item.folderContent;
            
            _desktopMinigame = desktopMinigame;

            TruncateLabel();
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
                        _desktopMinigame.OpenDesktopFolder(_contentScrObj, _fileName);
                        break;
                    case DesktopItemType.FolderInFolder:
                        _desktopMinigame.OpenFolder(_contentScrObj, _fileName);
                        break;
                    case DesktopItemType.Program:
                        _desktopMinigame.OpenProgram();
                        break;
                    case DesktopItemType.File:
                        _desktopMinigame.OpenFile(_fileName, _value, _documentSprite);
                        break;
                    case DesktopItemType.TargetFile:
                        _desktopMinigame.OpenTargetFile(_fileName, _value, _documentSprite);
                        break;
                }
            }
        }

        private void OnSelect(BaseEventData eventData)
        {
            // Display full name
            string fileName = "";
            
            if (_type == DesktopItemType.TargetFile)
            {
                fileName += "<b><i><color=red>";
            }

            fileName += _fileName;
            
            label.text = fileName;
        }
        
        private void OnDeselect(BaseEventData eventData)
        {
            // Display truncated name
            TruncateLabel();
        }
        
        private void TruncateLabel()
        {
            string prefix = "";
            
            if (_type == DesktopItemType.TargetFile)
            {
                prefix = "<b><i><color=red>";
            }
            
            label.text = prefix + _fileName;
            label.ForceMeshUpdate();

            float maxHeight = label.rectTransform.rect.height;

            if (label.preferredHeight <= maxHeight)
                return;

            string truncated = _fileName;

            while (truncated.Length > 0)
            {
                truncated = truncated.Substring(0, truncated.Length - 1);
                label.text = prefix + truncated + "...";
                label.ForceMeshUpdate();

                if (label.preferredHeight <= maxHeight)
                    break;
            }
        }
    }
}
