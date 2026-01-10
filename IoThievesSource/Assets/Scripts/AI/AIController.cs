using System;
using AI.AI_States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [Header("AI Settings")]
        [SerializeField] private AINames startState;
        [SerializeField] private AIState _currentState;
        [SerializeField] private NpcScriptableObject _NpcDatas;
        public event Action<AIState> OnStateChanged; 
        public event Action<AIState> BeforeStateChanged; 
        
        public EnemyAdapter enemyAdapter;

        #region DebugVariables
        [Header("Debug Variables")]
        public bool Stun = false;
        #endregion
        
        //public AIState CurrentState => _currentState;

        public AIState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value) return;
                
                BeforeStateChanged?.Invoke(_currentState);
                _currentState = value;
                OnStateChanged?.Invoke(_currentState);
            }
        }

        private StateFactory _stateFactory;
        public StateFactory StateFactory => _stateFactory;
        
        public NpcScriptableObject NpcDatas => _NpcDatas;

        private AIState _previousState;

        public void Initialize()
        {
            _stateFactory = new StateFactory(this);
            _previousState = _stateFactory.GetState(AINames.Idle);
        }
        
        private void Update()
        {
            if (Stun)
            {
                ChangeState(AINames.StunnedState);
                Stun = false; 
            }
            _currentState?.Execute();
        }

        public AINames GetStartState()
        {
            return startState;
        }
        
        public void ChangeState(AINames state)
        {
            if (CurrentState is StunnedState)
                return;
            
            AIState targetState = _stateFactory.GetState(state);
            if (targetState == _currentState) //Only one instance of the same state is stored in the factory
                return;
            
            _currentState?.Exit();
            _previousState = _currentState;
            _currentState = targetState;
            _currentState?.Enter();
        }

        public void PreviousState()
        {
           _currentState?.Exit();
           (_currentState, _previousState) = (_previousState, _currentState);
           _currentState?.Enter();
        }
        
    }
}