using System;
using System.Collections.Generic;
using AI.AI_States;
using Events;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        private Transform resetPosition;
        
        [Header("AI Settings")]
        [SerializeField] private AINames startState;
        [SerializeField] private AIState _currentState = null;
        [SerializeField] private NpcScriptableObject _NpcDatas;
        [SerializeField] private WalkableArea _walkableArea;
        public event Action<AIState> OnStateChanged; 
        public event Action<AIState> BeforeStateChanged; 
        
        public EnemyAdapter enemyAdapter;

        #region DebugVariables
        [Header("Debug Variables")]
        public bool Stun = false;
        #endregion
        

        public AIState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value) return;
                
                BeforeStateChanged?.Invoke(_currentState);
                _currentState = value;
                if (_currentState is not null)
                    OnStateChanged?.Invoke(_currentState);
            }
        }
        
        public WalkableArea WalkableArea => _walkableArea;

        private StateFactory _stateFactory;
        public StateFactory StateFactory => _stateFactory;
        
        public NpcScriptableObject NpcDatas => _NpcDatas;

        private AIState _previousState;

        public void Awake()
        {
            resetPosition = transform;
        }
        public void Initialize()
        {
            transform.position = resetPosition.position;
            transform.rotation = resetPosition.rotation;
            CurrentState = null;
            _stateFactory = new StateFactory(this);
            _previousState = _stateFactory.GetState(AINames.Idle);
            CurrentState = _stateFactory.GetState(AINames.Idle);
            enemyAdapter.AnimationController.Initialize();
            _walkableArea = gameObject.GetComponentInParent<WalkableArea>();
        }
        
        private void Update()
        {
            //print(gameObject.name + ": Update the "+ _currentState.GetType().Name);
            if (Stun)
            {
                ChangeState(AINames.StunnedState);
                Stun = false; 
            }
            CurrentState?.Execute();
        }

        public AINames GetStartState()
        {
            return startState;
        }
        
        public void ChangeState(AINames state)
        {
            if (CurrentState is (StunnedState or GoToDestination))
            {
                print("Non cambio stato poichè sono stunnato oppure sto andando in un punto specifico e davvero non mi posso fermare");
                return;
            }
            
            AIState targetState = _stateFactory.GetState(state);
            if (targetState == CurrentState) //Only one instance of the same state is stored in the factory
                return;
            
            CurrentState?.Exit();
            _previousState = CurrentState;
            CurrentState = targetState;
            CurrentState?.Enter();
        }

        public void PreviousState()
        {
           CurrentState?.Exit();
           if (_previousState is PatrolState && WalkableArea is not null)
               (CurrentState, _previousState) = (StateFactory.GetState(AINames.BackToWalkableArea), CurrentState);
           else
                (CurrentState, _previousState) = (_previousState, CurrentState);
           CurrentState?.Enter();
        }
        
    }
}