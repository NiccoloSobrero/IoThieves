using UnityEngine;

namespace AI.AI_States
{
    public class StunnedState : AIState
    {
        private float _elapsedTime;
        private const float TotalStunTime = 2.0f;
        private bool _billboard;

        private GameObject _vfx;
        
        public StunnedState(AIController npc) : base(npc) {}
        

        public override void Enter()
        {
            Npc.enemyAdapter.AnimationController.SetStunned(true);
            Npc.enemyAdapter.AnimationController.SetWalking(false);
            _elapsedTime = 0;
            
            Npc.enemyAdapter.AlertManager.Pause = true;
            Npc.enemyAdapter.NpcTrigger.enabled = false;
            
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(false);
            Npc.enemyAdapter.SuspicionSphere.gameObject.SetActive(false);
            
            _billboard = Npc.enemyAdapter.AlertManager._billboardVisible;
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(false);
            
            if (Npc.stunVfx != null)
            {
                Vector3 spawnPos = Npc.transform.position + new Vector3(0, 2.2f, 0);
                _vfx = Object.Instantiate(Npc.stunVfx, spawnPos, Quaternion.Euler(90,0,0));
                _vfx.transform.SetParent(Npc.transform); 
            }
        }

        public override void Execute()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= TotalStunTime)
            {
                Npc.PreviousState();
                _elapsedTime = 0;
            }
        }

        public override void Exit()
        {
            Npc.enemyAdapter.AlertManager.ShowAlertBillboard(_billboard); 
            Npc.enemyAdapter.SuspicionSphere.gameObject.SetActive(true);
            Npc.enemyAdapter.NpcTrigger.enabled = true;
            Npc.enemyAdapter.FovMesh.gameObject.SetActive(true);
            Npc.enemyAdapter.AlertManager.Pause = false;
            Npc.enemyAdapter.AnimationController.SetStunned(false);
            Npc.enemyAdapter.AnimationController.SetWalking(false);

            if (_vfx != null)
            {
                Object.Destroy(_vfx);
                _vfx = null;
            }
        }

        public void ResetCounter()
        {
           _elapsedTime = 0; 
        }
    }
}