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
        
        [Header("VFX")]
        [SerializeField] private NpcVfxController npcVfxController;
        
        public AlertManager AlertManager => alertManager;
        public AIController AIController => npc;
        public NpcTrigger NpcTrigger => SphereFOV;
        public EnemyFOVMesh FovMesh => fovMesh;
        public SuspicionSphere SuspicionSphere => suspicionSphere;
        public NpcAnimationController AnimationController => animationController;
        public NpcVfxController NpcVfxController => npcVfxController;

        private void Awake()
        {
            EventsManager.OnGameOverTriggered.AddListener(DisableAll);
            EventsManager.OnRestartGame.AddListener(ResetNpc);
        }

        private void Start()
        {
            ResetNpc();
        }

        private void DisableAll()
        {
            Debug.Log(gameObject.name + " DisableAll");
            npc.enabled = false;
            alertManager.enabled = false;
            SphereFOV.enabled = false;
            fovMesh.enabled = false;
            SuspicionSphere.enabled = false;
            SuspicionSphere.EnableSphere(false);
            animationController.enabled = false;

            npcVfxController?.StunVfx(false); //is not null OR !=null
        }

        private void ResetNpc()
        {
            Debug.Log(gameObject.name + " Resetting Npc");
            
            npc.enabled = true;
            npc.Initialize();
            //Debug.Log(gameObject.name + " AICONTROLLER RESET ");
            
            alertManager.Initialize();
            alertManager.enabled = true;
            //Debug.Log(gameObject.name + " ALERTMANAGER RESET ");
            
            SphereFOV.Initialize();
            SphereFOV.enabled = true;
            //Debug.Log(gameObject.name + " NPCDeathTrigger RESET ");
            
            fovMesh.enabled = true;
            fovMesh.gameObject.SetActive(true);
            //Debug.Log(gameObject.name + " FOVMesh RESET ");
            
            SuspicionSphere.enabled = true;
            SuspicionSphere.gameObject.SetActive(true);
            SuspicionSphere.Initialize();
            //Debug.Log(gameObject.name + " SUSPICION SPHERE RESET ");
            
            animationController.Initialize();
            animationController.enabled = true;
            //Debug.Log(gameObject.name + " AnimationController RESET ");
            
            SuspicionSphere.EnableSphere(true);
            //Debug.Log(gameObject.name + " SUSPICION SPHERE ENABLED ");

            npcVfxController?.Initialize(); //is not null OR !=null
        }
    }
}

 