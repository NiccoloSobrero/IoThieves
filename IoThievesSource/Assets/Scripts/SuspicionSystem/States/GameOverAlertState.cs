using Managers;
using SuspicionSystem;
using UnityEngine;

public class GameOverAlertState : AlertState
{
    public override AlertLevel Level => AlertLevel.GameOver;

    public override void Enter(AlertManager ctx)
    {
        Debug.Log("Entered GameOverAlertState");
        
        // GameManager2.Instance.TriggerGameOver();
    }

    public override void Exit(AlertManager ctx)
    {
        Debug.Log("Exited GameOverAlertState");
    }

    public override void Tick(AlertManager ctx, float dt)
    {
        //stato di errore 
    }
}
