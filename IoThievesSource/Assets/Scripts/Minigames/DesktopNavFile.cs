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
        
        private readonly Vector2 _imageSizeOffset = new(200, 200);
        private readonly Vector2 _windowSizeOffset = new(200, 100);
        
        public void AdaptToImage(Sprite documentSprite, string fileName, RectTransform parentRect)
        {
            fileNameText.text = fileName;
            documentImage.sprite = documentSprite;
            
            Vector2 maxWindowSize = parentRect.rect.size - _windowSizeOffset;

            float width = documentImage.sprite.rect.width;
            float height = documentImage.sprite.rect.height;
            float widthHeightRatio = width / height;

            float newWidth;
            float newHeight;

            if (width > height)
            {
                newWidth = maxWindowSize.x - _imageSizeOffset.x;
                newHeight = newWidth / widthHeightRatio;

                if (newHeight > maxWindowSize.y - _imageSizeOffset.y)
                {
                    newHeight = maxWindowSize.y - _imageSizeOffset.y;
                    newWidth = newHeight * widthHeightRatio;
                }
            }
            else
            {
                newHeight = maxWindowSize.y - _imageSizeOffset.y;
                newWidth = newHeight * widthHeightRatio;
            }
            
            documentImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
            
            windowRect.sizeDelta = documentImageRect.sizeDelta + _imageSizeOffset;
            
            TruncateLabel(fileNameText.text);
        }
        
        public void RefreshDimensions()
        {
            Vector2 maxWindowSize = windowRect.rect.size;

            float width = documentImage.sprite.rect.width;
            float height = documentImage.sprite.rect.height;
            float widthHeightRatio = width / height;

            float newWidth;
            float newHeight;

            if (width > height)
            {
                newWidth = maxWindowSize.x - _imageSizeOffset.x;
                newHeight = newWidth / widthHeightRatio;

                if (newHeight > maxWindowSize.y - _imageSizeOffset.y)
                {
                    newHeight = maxWindowSize.y - _imageSizeOffset.y;
                    newWidth = newHeight * widthHeightRatio;
                }
            }
            else
            {
                newHeight = maxWindowSize.y - _imageSizeOffset.y;
                newWidth = newHeight * widthHeightRatio;
            }
            
            documentImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
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