using System;
using System.Collections;
using AI;
using Events;
using SuspicionSystem.States;
using UnityEngine;
using UnityEngine.UI;

namespace SuspicionSystem
{
    public class AlertManager : MonoBehaviour
    {
    #region Alert icon fill constants
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
    #endregion

        [Header("Icon references")]
        [SerializeField] private GameObject alertBillboard;
        [SerializeField] private GameObject alertParent;

        [SerializeField] private RectMask2D questionMarkRectMask;
        [SerializeField] private RectMask2D exclamationMark1RectMask;
        [SerializeField] private RectMask2D exclamationMark2RectMask;
        
        private float QuestionMarkProgress =>Level0State.Suspicion;
        private float ExclamationMark1Progress => Level1State.Suspicion;
        private float ExclamationMark2Progress => Level2State.Suspicion;

        [SerializeField] private float timeBeforeAlertDecrease = 5f;
        
        private bool _canIncreaseSuspicion;
        private bool _canDecreaseSuspicion;

        public bool _billboardVisible;
        
        public bool IsPlayerInRange { get; private set; }
        public bool CanDecreaseSuspicion { get; private set; }
        
        public AlertState Level0State   { get; private set; }
        public AlertState Level1State { get; private set; }
        public AlertState Level2State { get; private set; }
        public AlertState GameOverState { get; private set; }
        
        private AlertState _currentState;
        
        private Coroutine _decayDelayRoutine;
        
        //Added by Fede
        public EnemyAdapter EnemyAdapter { get; set; }
        private bool _pause = false;
        public bool Initializing;
        public bool Pause
        {
            get => _pause;
            set => _pause = value;
        }

        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            Initializing = true;
            Debug.Log("Initializing Alert Manager");
            EnemyAdapter = GetComponent<EnemyAdapter>();
            
            Level0State = new Level0AlertState();
            Level1State = new Level1AlertState();
            Level2State = new Level2AlertState();
            GameOverState = new GameOverAlertState();
            
            SetUpConstants();
            try
            {
                EnemyAdapter.AIController.Initialize();
                print(gameObject.name + "AIController initialized");
            }
            catch (Exception e)
            {
            }

            Pause = false;
            ChangeState(Level0State);
            print("state changed ");
            Initializing = false;
        }

        private void Update()
        {
            if (Pause)
                return;
            _currentState?.Tick(this, Time.deltaTime);
            UpdateBars();
        }

        public void SetAlertedState(bool isAlerted)
        {
            IsPlayerInRange = isAlerted;
            
            if (!isAlerted)
            {
                if (_decayDelayRoutine != null)
                {
                    StopCoroutine(_decayDelayRoutine);
                }
                
                _decayDelayRoutine = StartCoroutine(DelayedDecay());
            }
            else
            {
                if (_decayDelayRoutine != null)
                {
                    StopCoroutine(_decayDelayRoutine);
                }
                
                CanDecreaseSuspicion = false;
            }
        }

        private IEnumerator DelayedDecay()
        {
            yield return new WaitForSeconds(timeBeforeAlertDecrease);
            
            _decayDelayRoutine = null;
            
            CanDecreaseSuspicion = true;
        }

        public void ChangeState(AlertState newState)
        {
            Debug.Log(newState.ToString());
            if (newState == _currentState || newState == null) return;

            _currentState?.Exit(this);
            _currentState = newState;
            _currentState.Enter(this);
        }

        private void UpdateBars()
        {
            float questionMarkPadding = - (_questionMarkRectMax - _questionMarkRectMin) * QuestionMarkProgress + _questionMarkRectMax;
            float exclamationMark1Padding = - (_exclamationMark1RectMax - _exclamationMark1RectMin) * ExclamationMark1Progress + _exclamationMark1RectMax;
            float exclamationMark2Padding = - (_exclamationMark2RectMax - _exclamationMark2RectMin) * ExclamationMark2Progress + _exclamationMark2RectMax;
            
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

        public void ShowAlertBillboard(bool show)
        {
            _billboardVisible = show;
            alertBillboard.SetActive(show);
        }
    }
}
