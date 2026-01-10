using System;
using UnityEngine;

namespace AI
{
    public abstract class AIState
    {
        protected AIController Npc;
        public event Action<bool> OnWalkingVarChanged;
        public event Action<bool> OnInteractVarChanged;

        public AIState(AIController npc) => Npc = npc;
        
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
        
        protected void RaiseOnWalkingVarChanged(bool value)
        {
            OnWalkingVarChanged?.Invoke(value);
        }

        protected void RaiseOnInteractVarChanged(bool value)
        {
            OnInteractVarChanged?.Invoke(value);
        }
    }
}