using System;
using UnityEngine;

namespace AI.AI_States
{
    public class StunnedState : AIState
    {
        private float elapsedTime;
        private float TotalStunTime = 2.0f;
        private bool billboard;
        public StunnedState(AIController npc) : base(npc) {}
        

        public override void Enter()
        {
            Npc.enemyAdapter.AnimationController.SetStunned(true);
            elapsedTime = 0;
            //DisableFovsD
            Debug.Log("StunnedEnemy");
            Npc.enemyAdapter.AlertManager.Pause = true;
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(false);
            Npc.enemyAdapter.NpcTrigger.enabled = false;
            Npc.enemyAdapter.SuspicionSphere.gameObject.SetActive(false);
            billboard = Npc.enemyAdapter.AlertManager._billboardVisible;
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(false);
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
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(billboard); 
            Npc.enemyAdapter.SuspicionSphere.gameObject.SetActive(true);
            Npc.enemyAdapter.NpcTrigger.enabled = true;
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(true);
            Npc.enemyAdapter.AlertManager.Pause = false;
            Npc.enemyAdapter.AnimationController.SetStunned(false);
        }

        public void ResetCounter()
        {
           elapsedTime = 0; 
        }
    }
}