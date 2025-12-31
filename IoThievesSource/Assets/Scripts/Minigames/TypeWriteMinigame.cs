using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minigames
{
    public class TypeWriteMinigame : MonoBehaviour
    {
        [SerializeField] private List<TypeWriteSection> typingSections;
        
        private int _currentIndex;
        
        private LevelObjectiveScrObj _objective;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                typingSections[_currentIndex].OnDeselect(null);
                
                _currentIndex = (_currentIndex + 1) % typingSections.Count;
                
                typingSections[_currentIndex].OnSelect(null);
            }
        }

        public void Init(LevelObjectiveScrObj objective)
        {
            _objective = objective;

            foreach (TypeWriteSection typeWriteSection in typingSections)
            {
                typeWriteSection.Init();
            }
        }

        public void Close()
        {
            _objective?.Complete();
        }
    }
}