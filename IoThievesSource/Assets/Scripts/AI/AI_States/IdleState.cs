using AI.AI_States.StateFamilies;

namespace AI.AI_States
{
    public class IdleState : AIState , IBaseState, IInterruptableState
    {
        public IdleState(AIController npc) : base(npc) {}

        public override void Enter() {}

        public override void Execute() {}

        public override void Exit() {}
  
    }
}