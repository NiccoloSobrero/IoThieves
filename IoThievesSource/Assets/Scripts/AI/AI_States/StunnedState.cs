using System;
using System.Collections;
using Enemy;
using Unity.VisualScripting.FullSerializer;
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
            elapsedTime = 0;
            //DisableFovsD
            Debug.Log("ORCODEDDIO MI HANNO STUNNATO");
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