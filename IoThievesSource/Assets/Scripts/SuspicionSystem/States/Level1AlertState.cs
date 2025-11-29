using UnityEngine;

namespace SuspicionSystem.States
{
    public class Level1AlertState : AlertState
    {
        public override AlertLevel Level => AlertLevel.Level1;
        
        public Level1AlertState() //AiControllerChangeState
        {
            AlertDecreaseSpeed = 0.02f;
            AlertIncreaseSpeed = 0.1f;
            
            AlertThreshold = 1f;
            Suspicion = 0f;
        }

        public override void Enter(AlertManager ctx)
        {
            Debug.Log("Entered Level1AlertState");
            ctx.ShowAlertBillboard(true);
        }

        public override void Exit(AlertManager ctx)
        {
            Debug.Log("Exited Level1AlertState");
        }

        public override void Tick(AlertManager ctx, float dt)
        {
            if (ctx.IsPlayerInRange)
            {
                Suspicion = Mathf.Clamp01(Suspicion + AlertIncreaseSpeed * dt);

                if (Suspicion >= AlertThreshold)
                {
                    Suspicion = 1f;
                    
                    ctx.ChangeState(ctx.Level2State); //Livello2 di suspiciousness per AI?? 
                }
            }
            else if (ctx.CanDecreaseSuspicion)
            {
                Suspicion = Mathf.Clamp01(Suspicion - AlertDecreaseSpeed * dt);

                if (Suspicion <= 0f)
                {
                    Suspicion = 0f;
                    
                    ctx.ChangeState(ctx.Level0State);
                    //AIController -> Change State
                }
            }
        }
    }
}