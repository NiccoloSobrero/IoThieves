using Enemy;
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

        [SerializeField] private NpcAnimationController animationController;
        // [SerializeField] private NpcTriggerConeOfDeath ConeOfDeath;  
    
        public AlertManager AlertManager => alertManager;
        public AIController AIController => npc;
        public NpcTrigger NpcTrigger => SphereFOV;
        // public NpcTriggerConeOfDeath NpcTriggerConeOfDeath => ConeOfDeath;
        public NpcAnimationController AnimationController => animationController;
    }
}

 