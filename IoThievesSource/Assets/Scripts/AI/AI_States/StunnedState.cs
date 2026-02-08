using System;
using UnityEngine;

namespace AI.AI_States
{
    public class StunnedState : AIState
    {
        private float elapsedTime;
        private float TotalStunTime = 2.0f;
        private bool billboard;

        private GameObject _vfx;
        
        
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
            
            if (Npc.stunVfx != null)
            {
                Vector3 spawnPos = Npc.transform.position + new Vector3(0, 2.2f, 0); //0, 1.8f, 0
                _vfx = GameObject.Instantiate(Npc.stunVfx, spawnPos, Quaternion.Euler(90,0,0));
                _vfx.transform.SetParent(Npc.transform); 
            }
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

            if (_vfx != null)
            {
                GameObject.Destroy(_vfx);
                _vfx = null;
            }
        }

        public void ResetCounter()
        {
           elapsedTime = 0; 
        }
    }
}