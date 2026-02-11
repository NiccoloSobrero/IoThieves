using System;
using AI.AI_States;
using UnityEngine;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        private Vector3 _resetPosition;
        private Quaternion _resetRotation;

        [Header("AI Settings")] [SerializeField]
        private AINames startState;

        [SerializeField] private NpcScriptableObject npcDatas;
        [SerializeField] private WalkableArea walkableArea;
        
        public Vector3 lastPosition;
        public Quaternion lastRotation;
        public event Action<AIState> OnStateChanged;
        public event Action<AIState> BeforeStateChanged;

        public EnemyAdapter enemyAdapter;

        public GameObject stunVfx;
        
        
        
        #region DebugVariables

        [Header("Debug Variables")] public bool stun = false;

        #endregion


        [SerializeField] private AINames currentNpcStateName;
        
        private AIState _currentState = null;
        private AIState _previousState;
        private AIState _previousRealState;
        
        public WalkableArea WalkableArea => walkableArea;

        private StateFactory _stateFactory;
        public StateFactory StateFactory => _stateFactory;
        
        public NpcScriptableObject NpcDatas => npcDatas;
        
        public bool matchRotationWhenComingBack = false;

        public AIState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState == value) return;
                
                BeforeStateChanged?.Invoke(_currentState);
                _currentState = value;
                currentNpcStateName = AIUtilities.RetrieveAIName(_currentState);
                if (_currentState is not null)
                    OnStateChanged?.Invoke(_currentState);
            }
        }

        public void Awake()
        {
            _resetPosition = transform.position;
            _resetRotation = transform.rotation;
            
            Debug.Log(gameObject.name + ": Awake WITH POSITIONS = " + transform.position);
        }
        
     
        public void Initialize()
        {
            Debug.Log(gameObject.name + " -->  Initializing AI Controller");
            
            if (_resetPosition != Vector3.zero || gameObject.activeSelf)
            {
                gameObject.transform.position = _resetPosition;
                gameObject.transform.rotation = _resetRotation;
                Debug.Log(gameObject.name + " ---> RESTORED TO: " + transform.position);
            }
            
            _currentState = null;
            _stateFactory = new StateFactory(this);
            
            _previousState = _stateFactory.GetState(AINames.Idle);
            CurrentState = _stateFactory.GetState(AINames.Idle);
            _previousRealState = _stateFactory.GetState(AINames.Idle);
            
            enemyAdapter.AnimationController.Initialize();
            walkableArea = gameObject.GetComponentInParent<WalkableArea>();
            
            lastPosition = transform.position;
        }
        
        private void Update()
        {
            if (stun)
            {
                ChangeState(AINames.StunnedState);
                stun = false; 
            }
            CurrentState?.Execute();
        }

        public AINames GetStartState()
        {
            return startState;
        }
        
        public void ChangeState(AINames state)
        {
            if(CurrentState is StunnedState stunnedState && state == AINames.StunnedState)
            {
                stunnedState.ResetCounter();
                return;
            }
            
            if (CurrentState is  GoToDestination) 
            {
                return;
            }
            
            AIState targetState = _stateFactory.GetState(state);
            if (targetState == CurrentState)
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
           else if (_currentState is StunnedState)
           {
               CurrentState = _previousState;
           }
           else
           {
               (CurrentState, _previousState, _previousRealState) = (StateFactory.GetState(AINames.BackToPosition),_previousRealState, CurrentState);
           }
           CurrentState?.Enter();
        }
        
    }
}