using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Minigames
{
    public class AlignTranslationMinigame : Minigame
    {
        [Header("Settings")]
        [SerializeField] private RectTransform targetLine;
        [SerializeField] private RectTransform controlledLine;
        [SerializeField] private Slider slider;
        
        [SerializeField] private float targetLineMaxY;
        [SerializeField] private float deltaToRetarget;
        [SerializeField] private float deltaToComplete;
        
        [SerializeField] private Color colorCompletion;
        [SerializeField] private Color originalColor;
        
        [SerializeField] private bool changedToCompleted;
        [SerializeField] private bool changedToOriginal;

        private void Awake()
        {
            InitMinigame();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                InitMinigame();
            }
            
            controlledLine.anchoredPosition = new Vector2(0, slider.value);

            CheckColor();
        }

        private void CheckColor()
        {
            if (changedToCompleted && changedToOriginal) return;
            
            if (CheckCompletion())
            {
                controlledLine.gameObject.GetComponent<Image>().color = colorCompletion;
            }
            else
            {
                controlledLine.gameObject.GetComponent<Image>().color = originalColor;
            }
            
            changedToCompleted = !changedToCompleted;
            changedToOriginal = !changedToOriginal;
        }

        public override void InitMinigame()
        {
            float height = slider.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2f;
            slider.minValue = -height + slider.handleRect.sizeDelta.y / 2;
            slider.maxValue = height - slider.handleRect.sizeDelta.y / 2;
            
            targetLine.anchoredPosition = new Vector2(0, Random.Range(-targetLineMaxY, targetLineMaxY));
            
            float randControlledLinePos = Random.Range(slider.minValue, slider.maxValue);

            while (Mathf.Abs(randControlledLinePos - targetLine.anchoredPosition.y) < deltaToRetarget)
            {
                randControlledLinePos = Random.Range(slider.minValue, slider.maxValue);
            }
            
            slider.value = randControlledLinePos;
            controlledLine.anchoredPosition = new Vector2(randControlledLinePos, 0);
        }

        public override bool CheckCompletion()
        {
            return Mathf.Abs(controlledLine.anchoredPosition.y - targetLine.anchoredPosition.y) < deltaToComplete;
        }
    }
}
