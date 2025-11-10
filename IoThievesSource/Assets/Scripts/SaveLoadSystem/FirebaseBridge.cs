using Events;
using UnityEngine;

namespace SaveLoadSystem
{
    public class FirebaseBridge : MonoBehaviour
    {
        private void Awake()
        {
            FirebaseWeb.InitTarget(gameObject.name);
        }
        
#region JS Callbacks/Fallbacks
        public void OnFirebaseSaveOK(string data)
        {
            Debug.Log("Save OK");
            EventsManager.OnGameSaved?.Invoke(data);
        }
            
        public void OnFirebaseSaveError(string err) 
        {
            Debug.LogError("Save error: " + err);
            EventsManager.OnGameSaveError?.Invoke(err);
        }
        
        public void OnFirebaseLoadError(string err)
        {
            Debug.LogError("Load error: " + err);
            EventsManager.OnGameLoadError?.Invoke(err);
        }

        public void OnFirebaseDataLoaded(string json)
        {
            Debug.Log("Loaded: " + json);
            EventsManager.OnGameLoaded?.Invoke(json);
        }
#endregion
    }
}
