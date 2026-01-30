using System.Collections.Generic;
using AI;
using Events;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private static List<EnemyAdapter> enemies;
        public static EnemyManager Instance;
            
            
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}