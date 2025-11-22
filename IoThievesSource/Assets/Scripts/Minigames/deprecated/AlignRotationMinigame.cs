using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Minigames
{
    public class AlignRotationMinigame : Minigame
    {
        
        [Header("Settings")]
        [SerializeField] private RectTransform targetLine;
        [SerializeField] private RectTransform controlledLine;
        [SerializeField] private Slider slider;
        
        [SerializeField] private float targetLineMaxAngle;
        [SerializeField] private float controlledLineMaxAngle;
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
            
            controlledLine.rotation = Quaternion.Euler(0, 0, slider.value);

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
            slider.minValue = -controlledLineMaxAngle;
            slider.maxValue = controlledLineMaxAngle;
            
            targetLine.rotation = Quaternion.Euler(0, 0, Random.Range(-targetLineMaxAngle, targetLineMaxAngle));
            
            float randControlledLineRot = Random.Range(-controlledLineMaxAngle, controlledLineMaxAngle);

            while (Mathf.Abs(randControlledLineRot - targetLine.rotation.z) < deltaToRetarget)
            {
                randControlledLineRot = Random.Range(-controlledLineMaxAngle, controlledLineMaxAngle);
            }
            
            slider.value = randControlledLineRot;
            controlledLine.rotation = Quaternion.Euler(0, 0, randControlledLineRot);
        }
        
        public override bool CheckCompletion()
        {
            return Quaternion.Angle(controlledLine.rotation, targetLine.rotation) < deltaToComplete;
        }
    }
}
