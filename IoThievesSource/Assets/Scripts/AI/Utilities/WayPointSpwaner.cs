using System.Collections.Generic;
using UnityEngine;

namespace AI.Utilities
{
    public class WayPointSpwaner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject prefabToSpawn;
        [SerializeField] private int spawnCount = 10;
    
        [Header("Spawn Area")]
        [SerializeField] private Vector3 spawnAreaCenter = Vector3.zero;
        [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    
        [Header("Distance Check")]
        [SerializeField] private float minDistanceBetweenSpawns = 2f;
        [SerializeField] private LayerMask collisionCheckLayers = ~0;
        [SerializeField] private int maxAttempts = 100;
    
        [Header("Debug")]
        [SerializeField] private bool showSpawnArea = true;
        [SerializeField] private bool showSpawnedPositions = true;

        private List<Vector3> spawnedPositions = new List<Vector3>();

        private void Start()
        {
            SpawnObjects();
        }

        public void SpawnObjects()
        {
            if (prefabToSpawn == null)
            {
                Debug.LogError("Prefab to spawn is not assigned!");
                return;
            }

            spawnedPositions.Clear();
            int successfulSpawns = 0;
        
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPos = FindSafeSpawnPosition();
            
                if (spawnPos != Vector3.positiveInfinity)
                {
                    Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
                    spawnedPositions.Add(spawnPos);
                    successfulSpawns++;
                }
                else
                {
                    Debug.LogWarning($"Could not find safe position for spawn {i + 1} after {maxAttempts} attempts");
                }
            }
        
            Debug.Log($"Successfully spawned {successfulSpawns}/{spawnCount} objects");
        }

        private Vector3 FindSafeSpawnPosition()
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector3 randomPos = GetRandomPosition();
            
                if (IsPositionSafe(randomPos))
                {
                    return randomPos;
                }
            }
        
            return Vector3.positiveInfinity; // Failed to find safe position
        }

        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float y = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
            float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        
            return spawnAreaCenter + new Vector3(x, y, z);
        }

        private bool IsPositionSafe(Vector3 position)
        {
            // Check for overlapping colliders in the scene
            Collider[] colliders = Physics.OverlapSphere(position, minDistanceBetweenSpawns / 2, collisionCheckLayers);
        
            if (colliders.Length > 0)
            {
                return false;
            }
        
            // Check distance from all previously spawned objects
            foreach (Vector3 spawnedPos in spawnedPositions)
            {
                float distance = Vector3.Distance(position, spawnedPos);
                if (distance < minDistanceBetweenSpawns)
                {
                    return false;
                }
            }
        
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (showSpawnArea)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
            }
        
            if (showSpawnedPositions && spawnedPositions != null)
            {
                Gizmos.color = Color.green;
                foreach (Vector3 pos in spawnedPositions)
                {
                    Gizmos.DrawWireSphere(pos, minDistanceBetweenSpawns / 2);
                }
            }
        }
    }
}