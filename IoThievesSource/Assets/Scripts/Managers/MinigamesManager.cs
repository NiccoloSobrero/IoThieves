using System;
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
        // private readonly List<MinigameToObject> _instantiatedMinigames = new();

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
            
            // MinigameToObject minigameElement = FindMinigameObject(minigamePrefab.GetComponent<Minigame>());
            //
            // if (minigameElement != null)
            // {
            //     minigameElement.window.SetActive(true);
            //     _currentMinigame = minigameElement.minigame;
            // }
            // else
            // {
            //     _currentMinigame = Instantiate(minigamePrefab, minigamesParent).GetComponent<Minigame>();
            //
            //     _instantiatedMinigames.Add(new MinigameToObject(_currentMinigame, _currentMinigame.gameObject));
            //
            //     _currentMinigame.InitMinigame();
            // }
            
            _currentMinigame = Instantiate(minigamePrefab, minigamesParent).GetComponent<Minigame>();
            
            _currentMinigame.InitMinigame();
        }

        public void CheckCompletion()
        {
            if (_currentMinigame.CheckCompletion())
            {
                //close minigame window
                // FindMinigameObject(_currentMinigame).window.SetActive(false);
                Destroy(_currentMinigame.gameObject);
                PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
                
                /*TODO: confirm device hacking and attract nearby enemies*/
                Debug.Log("Minigame check complete");
            }
        }

        // private MinigameToObject FindMinigameObject(Minigame minigame)
        // {
        //     for (int i = 0; i < _instantiatedMinigames.Count; i++)
        //     {
        //         if (_instantiatedMinigames[i].minigame == minigame)
        //         {
        //             return _instantiatedMinigames[i];
        //         }
        //     }
        //     return null;
        // }
    }

    [Serializable]
    public class MinigameToObject
    {
        public Minigame minigame;
        public GameObject window;
        
        public MinigameToObject(Minigame minigame, GameObject window)
        {
            this.minigame = minigame;
            this.window = window;
        }
    }
}
