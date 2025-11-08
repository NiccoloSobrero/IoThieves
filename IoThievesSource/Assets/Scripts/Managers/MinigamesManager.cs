using System;
using System.Collections.Generic;
using Events;
using Minigames;
using UnityEngine;

namespace Managers
{
    public class MinigamesManager : MonoBehaviour
    {
        public static MinigamesManager Instance;
        [SerializeField] private Transform minigamesParent;
        private Minigame _currentMinigame;
        private List<MinigameToObject> _instantiatedMinigames = new List<MinigameToObject>();

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

        public void CheckCompletion()
        {
            if (_currentMinigame.CheckCompletion())
            {
                //close minigame window
                FindMinigameObject(_currentMinigame).SetActive(false);
                
                /*TODO: confirm device hacking*/
                print("Evviva!!");
                
            }
        }

        private GameObject FindMinigameObject(Minigame minigame)
        {
            for (int i = 0; i < _instantiatedMinigames.Count; i++)
            {
                if (_instantiatedMinigames[i].minigame == minigame)
                {
                    return _instantiatedMinigames[i].window;
                }
            }
            return null;
        }
    }

    [Serializable]
    public class MinigameToObject
    {
        public GameObject window;
        public Minigame minigame;
    }
}
