using System;
using AnimationControllers;
using Enemy;
using Events;
using SuspicionSystem;
using UnityEngine;

namespace AI
{
   // [RequireComponent(typeof(AIController))]
    [RequireComponent(typeof(AlertManager))]
    //[RequireComponent(typeof(NpcAnimationController))]
    public class EnemyAdapter : MonoBehaviour
    {
        [SerializeField] private AIController npc;
        [SerializeField] private AlertManager alertManager;
        [SerializeField] private NpcTrigger SphereFOV;
        [SerializeField] private EnemyFOVMesh fovMesh;
        [SerializeField] private SuspicionSphere suspicionSphere;
        [SerializeField] private NpcAnimationController animationController;
        //[SerializeField] private NpcTriggerConeOfDeath ConeOfDeath;  
        
        public AlertManager AlertManager => alertManager;
        public AIController AIController => npc;
        public NpcTrigger NpcTrigger => SphereFOV;
        public EnemyFOVMesh FovMesh => fovMesh;
        public SuspicionSphere SuspicionSphere => suspicionSphere;
        //public NpcTriggerConeOfDeath NpcTriggerConeOfDeath => ConeOfDeath;
        public NpcAnimationController AnimationController => animationController;

        private void Awake()
        {
            EventsManager.OnGameOverTriggered.AddListener(DisableAll);
            EventsManager.OnRestartGame.AddListener(EnableAll);
        }

        private void DisableAll()
        {
            Debug.Log(gameObject.name + " DisableAll");
            npc.enabled = false;
            alertManager.enabled = false;
            SphereFOV.enabled = false;
            fovMesh.enabled = false;
            SuspicionSphere.enabled = false;
            animationController.enabled = false;
        }

        private void EnableAll()
        {
            Debug.Log(gameObject.name + " EnableAll");
            alertManager.enabled = true;
            npc.enabled = true;
            SphereFOV.enabled = true;
            fovMesh.enabled = true;
            SuspicionSphere.enabled = true;
            //animationController.enabled = true;
        }
    }
}

 