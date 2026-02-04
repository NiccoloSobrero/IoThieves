using System;
using AI.AI_States;
using UnityEngine;

namespace AI
{
        public class StateFactory
        {
            private IdleState idleState;
            private SuspiciousState suspiciousState;
            private PatrolState patrolState;
            private DeviceHackedState deviceHackedState;
            private NoiseHeardState noiseHeardState;
            private PathCyclingState pathCyclingState; 
            private StunnedState stunnedState;
            private GoToDestination goToDestination;
            private BackToPreviousPosition _backToPreviousPosition;
            public StateFactory(AIController controller)
            {
                idleState = new IdleState(controller);
                suspiciousState = new SuspiciousState(controller);
                patrolState = new PatrolState(controller);
                deviceHackedState = new DeviceHackedState(controller);
                noiseHeardState =  new NoiseHeardState(controller);
                pathCyclingState = new PathCyclingState(controller) ; 
                stunnedState = new StunnedState(controller);
                goToDestination = new GoToDestination(controller);
                _backToPreviousPosition = new BackToPreviousPosition(controller);
            }
            
            public AIState GetState(AINames stateName)
            {
                //Debug.Log("GetState" + stateName);
                switch (stateName)
                {
                    case AINames.Idle : return idleState;
                    case AINames.Suspicious : return suspiciousState;
                    case AINames.Patrol: return patrolState;
                    case AINames.DeviceHacked: return deviceHackedState;
                    case AINames.NoiseHeardState : return noiseHeardState;
                    case AINames.PathCycling : return pathCyclingState;
                    case AINames.StunnedState : return stunnedState;
                    case AINames.GoToDestination: return goToDestination;
                    case AINames.BackToPosition: return _backToPreviousPosition;
                    default: throw new ArgumentException($"Invalid state name: {stateName}.");
                }
            }
        }

}