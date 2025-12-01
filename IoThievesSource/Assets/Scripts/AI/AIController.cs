using AI.AI_States;
using UnityEngine;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [Header("AI Settings")]
        [SerializeField] private AINames startState;
        [SerializeField] private AIState _currentState;
        private StateFactory _stateFactory;

        private AIState _previousState;

        private void Awake()
        {
            _stateFactory = new StateFactory(this);
            _previousState = _stateFactory.GetState(AINames.Idle);
            ChangeState(startState);
        }
        
        private void Update()
        {
            _currentState?.Execute();
        /*
            if(Input.GetKeyDown(KeyCode.Space))     //Just for Testing Purposes
                if(_currentState is PatrolState)
                    ChangeState(AINames.Idle);
                else
                    ChangeState(AINames.Patrol);
        */
        }
        
        public void ChangeState(AINames state)
        {
            AIState targetState = _stateFactory.GetState(state);
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