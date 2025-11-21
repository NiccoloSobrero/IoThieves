using UnityEngine;

namespace SuspicionSystem.States
{
    public class Level2AlertState : AlertState
    {
        public override AlertLevel Level => AlertLevel.Level2;
        
        public Level2AlertState()
        {
            AlertDecreaseSpeed = 0.025f;
            AlertIncreaseSpeed = 0.15f;
            
            AlertThreshold = 1f;
            Suspicion = 0f;
        }

        public override void Enter(AlertManager ctx)
        {
            Debug.Log("Entered Level2AlertState");
        }

        public override void Exit(AlertManager ctx)
        {
            Debug.Log("Exited Level2AlertState");
        }

        public override void Tick(AlertManager ctx, float dt)
        {
            if (ctx.IsPlayerInRange)
            {
                Suspicion = Mathf.Clamp01(Suspicion + AlertIncreaseSpeed * dt);
            }
            else if (ctx.CanDecreaseSuspicion)
            {
                Suspicion = Mathf.Clamp01(Suspicion - AlertDecreaseSpeed * dt);

                if (Suspicion <= 0f)
                {
                    Suspicion = 0f;
                    
                    ctx.ChangeState(ctx.Level1State);
                }
            }
        }
    }
}