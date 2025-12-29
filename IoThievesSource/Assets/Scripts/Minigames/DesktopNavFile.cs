using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    public class DesktopNavFile : MonoBehaviour
    {
        [SerializeField] private Image documentImage;
        [SerializeField] private RectTransform documentImageRect;
        [SerializeField] private RectTransform windowRect;
        [SerializeField] private TextMeshProUGUI fileNameText;
        
        private readonly Vector2 _windowSizeOffset = new(200, 200);
        
        public void AdaptToImage(Sprite documentSprite, string fileName)
        {
            documentImage.sprite = documentSprite;
            documentImage.SetNativeSize();

            windowRect.sizeDelta = documentImageRect.sizeDelta + _windowSizeOffset;
            
            TruncateLabel(fileName);
        }
        
        private void TruncateLabel(string fileName)
        {
            fileNameText.text = fileName;
            fileNameText.ForceMeshUpdate();

            float maxWidth = fileNameText.rectTransform.rect.width;

            if (fileNameText.preferredWidth <= maxWidth)
                return;

            string truncated = fileName;

            while (truncated.Length > 0)
            {
                truncated = truncated.Substring(0, truncated.Length - 1);
                fileNameText.text = truncated + "...";
                fileNameText.ForceMeshUpdate();

                if (fileNameText.preferredWidth <= maxWidth)
                    break;
            }
        }
    }
}