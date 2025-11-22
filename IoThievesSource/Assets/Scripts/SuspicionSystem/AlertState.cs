namespace SuspicionSystem
{
    public abstract class AlertState
    {
        public abstract AlertLevel Level { get; }

        protected float AlertIncreaseSpeed;
        protected float AlertDecreaseSpeed;
        
        protected float AlertThreshold = 1f;

        public float Suspicion;
        
        public virtual void Enter(AlertManager ctx) { }

        public virtual void Exit(AlertManager ctx) { }

        // Called every Update()
        public abstract void Tick(AlertManager ctx, float deltaTime);
    }
}