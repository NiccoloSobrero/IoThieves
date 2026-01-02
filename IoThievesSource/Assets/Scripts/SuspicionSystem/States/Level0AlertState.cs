using AI;
using UnityEngine;

namespace SuspicionSystem.States
{
    public class Level0AlertState : AlertState
    {
        public override AlertLevel Level => AlertLevel.Level0;

        public Level0AlertState()
        {
            AlertDecreaseSpeed = 0f;
            AlertIncreaseSpeed = 0f;
            AlertThreshold = 0f;
            Suspicion = 1f;
        }

        public override void Enter(AlertManager ctx)
        {
            Debug.Log("Entered Level0AlertState");
            ctx.ShowAlertBillboard(false);

            AIController aiController = ctx.EnemyAdapter.AIController;
            aiController.ChangeState(aiController.GetStartState());
        }

        public override void Exit(AlertManager ctx)
        {
            Debug.Log("Exited Level0AlertState");
        }

        public override void Tick(AlertManager ctx, float dt)
        {
            if (ctx.IsPlayerInRange)
            {
                ctx.ChangeState(ctx.Level1State);
            }
        }
    }
}