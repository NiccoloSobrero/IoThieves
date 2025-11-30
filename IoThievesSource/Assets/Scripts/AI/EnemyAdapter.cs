using SuspicionSystem;
using UnityEngine;

namespace AI
{
    public class EnemyAdapter : MonoBehaviour
    {
        [SerializeField] public AIController npc;
        [SerializeField] public AlertManager alertManager;
    
        public AlertManager AlertManager => alertManager;
        public AIController AIController => npc;
    }
}

 