using System;
using AI.AI_States;

namespace AI
{
        public class StateFactory
        {
            private IdleState idleState;
            private SuspiciousState suspiciousState;
            private PatrolState patrolState;
            
            public StateFactory(AIController controller)
            {
                idleState = new IdleState(controller);
                suspiciousState = new SuspiciousState(controller);
                patrolState = new PatrolState(controller);
            }
            
            public AIState GetState(AINames stateName)
            {
                switch (stateName)
                {
                    case AINames.Idle : return idleState;
                    case AINames.Suspicious : return suspiciousState;
                    case AINames.Patrol: return patrolState;
                    default: throw new ArgumentException($"Invalid state name: {stateName}.");
                }
            }

        }

}