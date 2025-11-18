using UnityEngine;
using UnityEngine.UI;

namespace SuspicionSystem
{
    public class AlertManager : MonoBehaviour
    {
        private readonly float _questionMarkRectMaxRef = 0.76f;
        private readonly float _questionMarkRectMinRef = 0.27f;
    
        private readonly float _exclamationMark1RectMaxRef = 0.78f;
        private readonly float _exclamationMark1RectMinRef = 0.2f;

        private readonly float _exclamationMark2RectMaxRef = 0.79f;
        private readonly float _exclamationMark2RectMinRef = 0.14f;
        
        private float _questionMarkRectMax;
        private float _questionMarkRectMin;
    
        private float _exclamationMark1RectMax;
        private float _exclamationMark1RectMin;

        private float _exclamationMark2RectMax;
        private float _exclamationMark2RectMin;

        [SerializeField] private GameObject alertParent;

        [SerializeField] private RectMask2D questionMarkRectMask;
        [SerializeField] private RectMask2D exclamationMark1RectMask;
        [SerializeField] private RectMask2D exclamationMark2RectMask;
        
        [SerializeField, Range(0f, 1f)] private float questionMarkProgress;
        [SerializeField, Range(0f, 1f)] private float exclamationMark1Progress;
        [SerializeField, Range(0f, 1f)] private float exclamationMark2Progress;

        private void Awake()
        {
            SetUpConstants();
        }

        private void Update()
        {
            UpdateBars();
        }

        private void UpdateBars()
        {
            float questionMarkPadding = - (_questionMarkRectMax - _questionMarkRectMin) * questionMarkProgress + _questionMarkRectMax;
            float exclamationMark1Padding = - (_exclamationMark1RectMax - _exclamationMark1RectMin) * exclamationMark1Progress + _exclamationMark1RectMax;
            float exclamationMark2Padding = - (_exclamationMark2RectMax - _exclamationMark2RectMin) * exclamationMark2Progress + _exclamationMark2RectMax;
            
            questionMarkRectMask.padding = new Vector4(0, 0, 0, questionMarkPadding);
            exclamationMark1RectMask.padding = new Vector4(0, 0, 0, exclamationMark1Padding);
            exclamationMark2RectMask.padding = new Vector4(0, 0, 0, exclamationMark2Padding);
        }

        private void SetUpConstants()
        {
            float multiplier = alertParent.GetComponent<RectTransform>().rect.width;
            
            _questionMarkRectMax = multiplier * _questionMarkRectMaxRef;
            _questionMarkRectMin = multiplier * _questionMarkRectMinRef;
            
            _exclamationMark1RectMax = multiplier * _exclamationMark1RectMaxRef;
            _exclamationMark1RectMin = multiplier * _exclamationMark1RectMinRef;
            
            _exclamationMark2RectMax = multiplier * _exclamationMark2RectMaxRef;
            _exclamationMark2RectMin = multiplier * _exclamationMark2RectMinRef;
        }
    }
}
