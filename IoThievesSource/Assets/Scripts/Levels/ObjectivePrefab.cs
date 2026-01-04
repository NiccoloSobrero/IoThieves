using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class ObjectivePrefab : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI objectiveDescription;
        [SerializeField] private Image checkImage;
        [SerializeField] private Image boxImage;
        
        [SerializeField] private Color completedColor;
        [SerializeField] private Color uncompletedColor;

        public void Initialize(string description)
        {
            objectiveDescription.text = description;
            
            checkImage.gameObject.SetActive(false);
            boxImage.gameObject.SetActive(true);
            
            boxImage.color = uncompletedColor;
            objectiveDescription.color = uncompletedColor;
        }

        public void UpdateText(string description)
        {
            objectiveDescription.text = description;
        }
        
        public void SetCompleted()
        {
            checkImage.gameObject.SetActive(true);
            
            boxImage.color = completedColor;
            checkImage.color = completedColor;
            objectiveDescription.color = completedColor;

            objectiveDescription.text = "<s>" + objectiveDescription.text;
        }
    }
}