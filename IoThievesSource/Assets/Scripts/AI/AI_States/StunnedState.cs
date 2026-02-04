using System;
using UnityEngine;

namespace AI.AI_States
{
    public class StunnedState : AIState
    {
        private float elapsedTime;
        private float TotalStunTime = 2.0f;
        public StunnedState(AIController npc) : base(npc) {}
        

        public override void Enter()
        {
            Npc.enemyAdapter.AnimationController.SetStunned(true);
            elapsedTime = 0;
            //DisableFovsD
            Debug.Log("StunnedEnemy");
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(false);
            Npc.enemyAdapter.SuspicionSphere.gameObject.SetActive(false);
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(false);
            Npc.enemyAdapter.AlertManager.Pause = true;

        }

        public override void Execute()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= TotalStunTime)
            {
                Npc.PreviousState();
            }
        }

        public override void Exit()
        {
            Npc.enemyAdapter.AnimationController.SetStunned(false);
            Npc.enemyAdapter.SuspicionSphere.gameObject.SetActive(false);
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(true);
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(true); 
            Npc.enemyAdapter.AlertManager.Pause = false;
        }
    }
}