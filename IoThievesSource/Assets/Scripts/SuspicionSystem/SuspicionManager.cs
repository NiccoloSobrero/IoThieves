using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuspicionSystem
{
    public class SuspicionManager : MonoBehaviour
    {
        public static SuspicionManager Instance;
        
        [Header("References")]
        [SerializeField] private Slider suspicionSlider;
        [SerializeField] private Image sliderFillImage;
        [SerializeField] private Gradient suspicionGradient;

        [Header("Settings")]
        [SerializeField] private float maxSuspicionLevel = 10f;
        [SerializeField] private float timeBeforeSuspicionDecrease = 5f;
        [SerializeField] private float suspicionDecreaseSpeed = 0.25f;
        
        private float _currentSuspicionLevel;
        public float CurrentSuspicionLevel => _currentSuspicionLevel;
        
        private bool _canDecreaseSuspicion;

        private readonly List<GameObject> _enemiesInRange = new();
        
        private Coroutine _suspicionDecreaseTimerCoroutine;

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
            
            SetMaxSuspicion(maxSuspicionLevel);
        }

        private void Update()
        {
            CheckAutoSuspicionDecrease();
        }

        private void CheckAutoSuspicionDecrease()
        {
            if (_canDecreaseSuspicion && _currentSuspicionLevel > 0)
            {
                _currentSuspicionLevel -= Time.deltaTime * suspicionDecreaseSpeed;

                if (_currentSuspicionLevel < 0)
                {
                    _currentSuspicionLevel = 0;

                    _canDecreaseSuspicion = false;
                }
                
                suspicionSlider.value = _currentSuspicionLevel;
            }
        }

        private void SetMaxSuspicion(float maxSuspicion)
        {
            maxSuspicionLevel = maxSuspicion;
            suspicionSlider.maxValue = maxSuspicion;
            suspicionSlider.minValue = 0;
            suspicionSlider.value = 0;
            
            UpdateSliderColor();
        }

        public void UpdateSliderColor()
        {
            sliderFillImage.color = suspicionGradient.Evaluate(suspicionSlider.value / maxSuspicionLevel);
        }

        public void IncreaseSuspicionLevel(float amount)
        {
            _currentSuspicionLevel += amount;
            suspicionSlider.value = _currentSuspicionLevel;
            
            _currentSuspicionLevel = Mathf.Clamp(_currentSuspicionLevel, 0, maxSuspicionLevel);
        }

        public void SetCurrentSuspicionLevel(float amount)
        {
            _currentSuspicionLevel = amount;
            suspicionSlider.value = _currentSuspicionLevel;
            
            UpdateSliderColor();
        }

        public void AddEnemy(GameObject enemy)
        {
            _enemiesInRange.Add(enemy);

            _canDecreaseSuspicion = false;

            if (_suspicionDecreaseTimerCoroutine != null)
            {
                StopCoroutine(_suspicionDecreaseTimerCoroutine);
            }
        }

        public void RemoveEnemy(GameObject enemy)
        {
            _enemiesInRange.Remove(enemy);

            if (_enemiesInRange.Count == 0)
            {
                _suspicionDecreaseTimerCoroutine = StartCoroutine(SuspicionDecreaseTimer());
            }
        }

        private IEnumerator SuspicionDecreaseTimer()
        {
            yield return new WaitForSeconds(timeBeforeSuspicionDecrease);

            _canDecreaseSuspicion = true;
        }
    }
}
