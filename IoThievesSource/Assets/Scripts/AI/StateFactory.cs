using System;
using AI.AI_States;

namespace AI
{
        public class StateFactory
        {
            private IdleState idleState;
            private SuspiciousState suspiciousState;
            private PatrolState patrolState;
            private DeviceHackedState deviceHackedState;
            private NoiseHeardState noiseHeardState;
            
            public StateFactory(AIController controller)
            {
                idleState = new IdleState(controller);
                suspiciousState = new SuspiciousState(controller);
                patrolState = new PatrolState(controller);
                deviceHackedState = new DeviceHackedState(controller);
            }
            
            public AIState GetState(AINames stateName)
            {
                switch (stateName)
                {
                    case AINames.Idle : return idleState;
                    case AINames.Suspicious : return suspiciousState;
                    case AINames.Patrol: return patrolState;
                    case AINames.DeviceHacked: return deviceHackedState;
                    case AINames.NoiseHeardState : return noiseHeardState;
                    default: throw new ArgumentException($"Invalid state name: {stateName}.");
                }
            }
        }

}