using UnityEngine;

namespace AI
{
    public abstract class AIState
    {
        protected AIController Npc;

        public AIState(AIController npc) => Npc = npc;
        
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}