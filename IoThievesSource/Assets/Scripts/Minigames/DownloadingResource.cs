using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    public class DownloadingResource : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resourceName;
        [SerializeField] private RectTransform resourceNameRect;
        [SerializeField] private Image barFillImage;
        [SerializeField] private GameObject completedCheckMark;
        [SerializeField] private Color completedColor;
        
        private const char NonBreakingSpace = '\u00A0';

        public void Init(string filePath)
        {
            resourceName.text = filePath;
            barFillImage.fillAmount = 0f;
            
            CheckTruncation();
        }
        
        public void UpdateProgress(float progress)
        {
            barFillImage.fillAmount = progress;
        }

        public void Complete()
        {
            completedCheckMark.SetActive(true);
            barFillImage.fillAmount = 1f;
            barFillImage.color = completedColor;
        }
        
        private void CheckTruncation()
        {
            resourceName.text = resourceName.text.Replace(' ', NonBreakingSpace);
            string fullText = resourceName.text;

            resourceName.ForceMeshUpdate();
            int lines = resourceName.textInfo.lineCount;

            if (lines <= 2)
                return;

            const string ellipsis = "...";
            int startIndex = 0;

            int maxTrim = fullText.Length - 1;

            while (lines > 2 && startIndex < maxTrim)
            {
                startIndex += 3;

                if (startIndex >= fullText.Length)
                    break;

                string tail = fullText.Substring(startIndex);
                resourceName.text = ellipsis + tail;

                resourceName.ForceMeshUpdate();
                lines = resourceName.textInfo.lineCount;
            }
        }
    }
}