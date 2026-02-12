namespace AI.AI_States.StateFamilies
{
    public interface IInterruptableState
    {
        public bool CanBeInterruptedBy(AIState other)
        {
            return true;
        }
    }
}