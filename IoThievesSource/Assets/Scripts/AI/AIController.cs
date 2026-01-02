using AI.AI_States;
using UnityEngine;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [Header("AI Settings")]
        [SerializeField] private AINames startState;
        [SerializeField] private AIState _currentState;
        
        public AIState CurrentState => _currentState;
        
        private StateFactory _stateFactory;
        public StateFactory StateFactory => _stateFactory;

        private AIState _previousState;

        private void Awake()
        {
            _stateFactory = new StateFactory(this);
            _previousState = _stateFactory.GetState(AINames.Idle);
          //  ChangeState(startState);
        }
        
        private void Update()
        {
            _currentState?.Execute();
        }

        public AINames GetStartState()
        {
            return startState;
        }
        public void ChangeState(AINames state)
        {
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