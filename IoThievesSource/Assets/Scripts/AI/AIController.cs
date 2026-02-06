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
        private Vector3 resetPosition;
        private Quaternion resetRotation;

        [Header("AI Settings")] [SerializeField]
        private AINames startState;

        [SerializeField] private NpcScriptableObject _NpcDatas;
        [SerializeField] private WalkableArea _walkableArea;
        
        public Vector3 lastPosition;
        public Quaternion lastRotation;
        public event Action<AIState> OnStateChanged;
        public event Action<AIState> BeforeStateChanged;

        public EnemyAdapter enemyAdapter;

        #region DebugVariables

        [Header("Debug Variables")] public bool Stun = false;

        #endregion


        [SerializeField] private AINames _currentNpcStateName;
        
        private AIState _currentState = null;
        private AIState _previousState;
        private AIState _previousRealState;
        
        public WalkableArea WalkableArea => _walkableArea;

        private StateFactory _stateFactory;
        public StateFactory StateFactory => _stateFactory;
        
        public NpcScriptableObject NpcDatas => _NpcDatas;
        
        public bool matchRotationWhenComingBack = false;

        public AIState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value) return;
                
                BeforeStateChanged?.Invoke(_currentState);
                _currentState = value;
                _currentNpcStateName = AIUtilities.RetrieveAIName(_currentState);
                if (_currentState is not null)
                    OnStateChanged?.Invoke(_currentState);
            }
        }

        public void Awake()
        {
            resetPosition = transform.position;
            resetRotation = transform.rotation;
            Debug.Log(gameObject.name + ": Awake WITH POSITIONS = " + transform.position);
        }
        
     
        public void Initialize()
        {
            Debug.Log(gameObject.name + " -->  Initializing AI Controller");
            Debug.Log(gameObject.name + " -->  ACTUAL POSITION IS " + transform.position + "\nWITH RESET POSITION AS " + resetPosition);
            gameObject.transform.position = resetPosition;
            gameObject.transform.rotation = resetRotation;
            Debug.Log(gameObject.name + " POSITION IS " + transform.position);
            _currentState = null;
            _stateFactory = new StateFactory(this);
            _previousState = _stateFactory.GetState(AINames.Idle);
            CurrentState = _stateFactory.GetState(AINames.Idle);
            _previousRealState = _stateFactory.GetState(AINames.Idle);
            enemyAdapter.AnimationController.Initialize();
            _walkableArea = gameObject.GetComponentInParent<WalkableArea>();
            lastPosition = transform.position;
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
            if(CurrentState is StunnedState stun)
            {
                stun.ResetCounter();
                return;
            }
            
            if (CurrentState is  GoToDestination) 
            {
                print("Non cambio perchè andando in un punto specifico e davvero non mi posso fermare");
                return;
            }
            
            AIState targetState = _stateFactory.GetState(state);
            if (targetState == CurrentState) //Only one instance of the same state is stored in the factory
                return;
            
            CurrentState?.Exit();
            _previousState = CurrentState;
            _previousRealState = CurrentState;
            CurrentState = targetState;
            
            lastPosition = transform.position;
            if (_previousState is IdleState)
            {
                lastRotation = transform.rotation;
                matchRotationWhenComingBack = true;
            }
            else
                matchRotationWhenComingBack = false;
            
            CurrentState?.Enter();
        }

        public void PreviousState()
        {
           CurrentState?.Exit();

           if (_currentState is BackToPreviousPosition)
           {
               (CurrentState, _previousState) = (_previousState, _previousRealState);
           }
           else
           {
               (CurrentState, _previousState, _previousRealState) = (StateFactory.GetState(AINames.BackToPosition),_previousRealState, CurrentState);
           }
           CurrentState?.Enter();
        }
        
    }
}