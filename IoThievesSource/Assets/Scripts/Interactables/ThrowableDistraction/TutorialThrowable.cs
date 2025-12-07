namespace Interactables.ThrowableDistraction
{
    public class TutorialThrowable : Throwable
    {
        protected override void HandleCollision()
        {
            Destroy(gameObject);
        }
    }
}