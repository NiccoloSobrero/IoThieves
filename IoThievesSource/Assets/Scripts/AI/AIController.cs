using System;
using System.Collections.Generic;
using AI.AI_States;
using AI.AI_States.StateFamilies;
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
        /*
        [SerializeField] private AINames previousNpcStateName;
        [SerializeField] private AINames previousRealNpcStateName;
        */
        
        private readonly Stack<StateStackElement> _stateStack = new Stack<StateStackElement>();

       
        private AIState _currentState = null;
        /*
       private AIState _previousState;
       private AIState _previousRealState;
       */
        
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
            _stateStack.Clear();
            
            _stateFactory = new StateFactory(this);

            _currentState = null;
            
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
        
        public void ChangeState(AINames nextStateName)
        {
            AIState nextState = _stateFactory.GetState(nextStateName);

            if (nextState == CurrentState)
            {
                if (CurrentState is StunnedState stun)
                    stun.ResetCounter();
                return;
            }
            
            if (CurrentState is not IInterruptableState &&
                CurrentState is not null)
            {
                return;
            }

            if (CurrentState is IInterruptableState interruptable &&
                !interruptable.CanBeInterruptedBy(nextState))
            {
                return;
            }
            
            if (CurrentState is not IRecoveryState &&
                CurrentState is not null)
            {
                _stateStack.Push(new StateStackElement(CurrentState, transform, CurrentState is IdleState));
            }
            
            //Questi stati sono tutti Base,Temporary,Interruptable
            if (CurrentState is ISuspendable susp)
                susp.Suspend();
            else 
                CurrentState?.Exit();
            
            CurrentState = nextState;
            CurrentState.Enter();
        }


        public void PreviousState()
        {
            CurrentState?.Exit();

            Debug.Log("PreviousState : " + CurrentState?.GetType().Name);
            //Se serve fare recovery 
            if (CurrentState is not IRecoveryState && IsRecoveryNeeded())
            {
                Debug.Log("I NEED A RECOVERY");
                CurrentState = _stateFactory.GetState(AINames.BackToPosition);
                CurrentState.Enter();
                Debug.Log(gameObject.name + " ###> " +CurrentState.GetType().Name);
                return;
            }
            
            //Eventuale fine della recovery
            if (_stateStack.Count == 0)
            {
                CurrentState = _stateFactory.GetState(startState);
                CurrentState.Enter();
                return;
            }
            
            StateStackElement element = _stateStack.Pop();
            
            transform.position = element.Position;
            if(element.RotationNeeded)
                transform.rotation = element.Rotation;

            CurrentState = element.State;
            
            if (CurrentState is ISuspendable susp)
                susp.Resume();
            else
                CurrentState.Enter();
        }

        private bool IsRecoveryNeeded()
        {
            if (_stateStack.TryPeek(out StateStackElement lastContext))
            {
                Debug.Log(gameObject.name + "Got a context there ####> "+ lastContext);
                Debug.Log(gameObject.name + " Actual position is " +  transform.position + "\n"
                          + Vector3.Distance(transform.position, lastContext.Position));
                return !(Vector3.Distance(transform.position, lastContext.Position) < 0.01f);
            }

            return false;
        }

        public StateStackElement PeekPreviousContext()
        {
            return _stateStack.Peek();
        }
        
        public struct StateStackElement
        {
            public AIState State;
            public Vector3 Position;
            public Quaternion Rotation;
            public bool RotationNeeded;

            public StateStackElement(AIState state, Transform t, bool hasRotation = false)
            {
                State = state;
                Position = t.position;
                Rotation = t.rotation;
                RotationNeeded = hasRotation;
            }

            public override string ToString()
            {
                return State.GetType().Name + " " + Position + " " + RotationNeeded + " " + Rotation;
            }
        }


    }
}