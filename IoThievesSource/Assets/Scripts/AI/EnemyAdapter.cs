using Enemy;
using SuspicionSystem;
using UnityEngine;

namespace AI
{
    public class EnemyAdapter : MonoBehaviour
    {
        [SerializeField] private AIController npc;
        [SerializeField] private AlertManager alertManager;
        [SerializeField] private NpcTrigger SphereFOV;
        // [SerializeField] private NpcTriggerConeOfDeath ConeOfDeath;  
    
        public AlertManager AlertManager => alertManager;
        public AIController AIController => npc;
        public NpcTrigger NpcTrigger => SphereFOV;
        // public NpcTriggerConeOfDeath NpcTriggerConeOfDeath => ConeOfDeath;
    }
}

 