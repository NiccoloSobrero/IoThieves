using Events;
using SuspicionSystem;
using UnityEngine;

namespace SaveLoadSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] private FirebaseBridge bridge;
        [SerializeField] private Transform player;
        
        private void OnEnable()
        {
            EventsManager.OnGameSaved.AddListener(HandleSavedJson);
            EventsManager.OnGameLoaded.AddListener(HandleLoadedJson);
            EventsManager.OnGameLoadError.AddListener(HandleLoadError);
            EventsManager.OnGameSaveError.AddListener(HandleSaveError);
            EventsManager.OnSaveGame.AddListener(SerializeAndSave);
        }

        private void OnDisable()
        {
            EventsManager.OnGameSaved.RemoveListener(HandleSavedJson);
            EventsManager.OnGameLoaded.RemoveListener(HandleLoadedJson);
            EventsManager.OnGameLoadError.RemoveListener(HandleLoadError);
            EventsManager.OnGameSaveError.RemoveListener(HandleSaveError);
            EventsManager.OnSaveGame.RemoveListener(SerializeAndSave);
        }
        
    #region Buttons calls
        public void OnClickSave()
        {
            SerializeAndSave();
        }

        public void OnClickLoad()
        {
            FirebaseWeb.Load();
        }
    #endregion

    #region Events calls
        private void HandleSavedJson(string data)
        {
            Debug.Log(data);
        }
        
        private void HandleLoadedJson(string json)
        {
            try
            {
                DeserializeAndLoad(json);
            }
            catch
            {
                Debug.LogWarning($"Could not deserialize: (Raw) {json}");
            }
        }

        private void HandleLoadError(string err)
        {
            Debug.LogWarning($"Load error: {err}");
        }
        private void HandleSaveError(string err)
        {
            Debug.LogWarning($"Save error: {err}");
        }
    #endregion

    #region The only things that can be touched
        private void SerializeAndSave()
        {
            GameSaveData payload = new GameSaveData(player);
            var json = JsonUtility.ToJson(payload);
            FirebaseWeb.SaveJson(json);
        }

        private void DeserializeAndLoad(string json)
        {
            GameSaveData data = string.IsNullOrEmpty(json) ? new GameSaveData() : JsonUtility.FromJson<GameSaveData>(json);
                
            Vector3 playerPos = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            Quaternion playerRot = Quaternion.Euler(data.playerRotation[0], data.playerRotation[1], data.playerRotation[2]);
                
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.SetPositionAndRotation(playerPos, playerRot);
            player.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    #endregion
    }
}
