namespace Levels
{
    public class TutorialObjective
    {
        private readonly TutorialLevelManager _manager;

        public TutorialObjective(TutorialLevelManager manager)
        {
            _manager = manager;
        }

        public void Complete()
        {
            _manager?.NextObjective();
        }
    }
}