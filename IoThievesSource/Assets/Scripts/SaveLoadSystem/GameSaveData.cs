using UnityEngine;

namespace SaveLoadSystem
{
    [System.Serializable]
    public class GameSaveData
    {
        public float[] playerPosition = new float[3];
        public float[] playerRotation = new float[3];

        public GameSaveData()
        {
            playerPosition[0] = 0;
            playerPosition[1] = 0;
            playerPosition[2] = 0;
            
            playerRotation[0] = 0;
            playerRotation[1] = 0;
            playerRotation[2] = 0;
        }
        
        public GameSaveData(Transform playerTransform)
        {
            playerPosition[0] = playerTransform.position.x;
            playerPosition[1] = playerTransform.position.y;
            playerPosition[2] = playerTransform.position.z;
            
            playerRotation[0] = playerTransform.rotation.eulerAngles.x;
            playerRotation[1] = playerTransform.rotation.eulerAngles.y;
            playerRotation[2] = playerTransform.rotation.eulerAngles.z;
        }
    }
}
