using Events;
using Minigames;
using UnityEngine;

namespace Managers
{
    public class MinigamesManager : MonoBehaviour
    {
        [SerializeField] private Transform minigamesParent;

        private void OnEnable()
        {
            EventsManager.OnHackableInteraction.AddListener(SpawnMinigame);
        }

        private void OnDisable()
        {
            EventsManager.OnHackableInteraction.RemoveListener(SpawnMinigame);
        }

        private void SpawnMinigame(GameObject minigamePrefab)
        {
            PlayerInputManager.Instance.SetContext(InputContext.Minigame);
            
            Minigame minigame = Instantiate(minigamePrefab, minigamesParent).GetComponent<Minigame>();
        
            minigame.InitMinigame();
        }
    }
}
