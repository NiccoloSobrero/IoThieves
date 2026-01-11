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
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(false);
            Npc.enemyAdapter.AlertManager.enabled = false;
            OnAndOffFovs(false);
            
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
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(true);
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(true);
            Npc.enemyAdapter.AlertManager.enabled = true;
            OnAndOffFovs(true);
        }

        private void OnAndOffFovs(bool active)
        { 
            String coroutineName = !active ? "ShrinkFov" : "EnlargeFov";
            
            //Npc.enemyAdapter.NpcTriggerConeOfDeath.gameObject.SetActive(active);
            Npc.enemyAdapter.NpcTrigger.StartCoroutine(coroutineName);
            // Npc.enemyAdapter.NpcTriggerConeOfDeath.StartCoroutine(coroutineName);
            //Npc.enemyAdapter.NpcTrigger.gameObject.SetActive(active);
        }

        
    }
}