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
            
            fileNameText.text = fileName;
        }
    }
}
