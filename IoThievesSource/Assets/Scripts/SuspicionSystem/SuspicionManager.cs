using UnityEngine;
using UnityEngine.UI;

namespace SuspicionSystem
{
    public class SuspicionManager : MonoBehaviour
    {
        public static SuspicionManager Instance;
        
        [SerializeField] private Slider suspicionSlider;
        [SerializeField] private Image sliderFillImage;
        [SerializeField] private Gradient suspicionGradient;

        private float _maxSuspicionLevel;
        private float _currentSuspicionLevel;
        
        public bool canDecreaseSuspicion;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            // Testing
            SetMaxSuspicion(10f);
        }

        private void Update()
        {
            CheckAutoSuspicionDecrease();
        }

        private void CheckAutoSuspicionDecrease()
        {
            if (canDecreaseSuspicion && _currentSuspicionLevel > 0)
            {
                _currentSuspicionLevel -= Time.deltaTime;

                if (_currentSuspicionLevel < 0)
                {
                    _currentSuspicionLevel = 0;
                }
                
                suspicionSlider.value = _currentSuspicionLevel;
            }
        }

        private void SetMaxSuspicion(float maxSuspicion)
        {
            _maxSuspicionLevel = maxSuspicion;
            suspicionSlider.maxValue = maxSuspicion;
            suspicionSlider.minValue = 0;
            suspicionSlider.value = 0;
            
            UpdateSliderColor();
        }

        public void UpdateSliderColor()
        {
            sliderFillImage.color = suspicionGradient.Evaluate(suspicionSlider.value / _maxSuspicionLevel);
        }

        public void IncreaseSuspicionLevel(float amount)
        {
            _currentSuspicionLevel += amount;
            suspicionSlider.value = _currentSuspicionLevel;
        }
    }
}
