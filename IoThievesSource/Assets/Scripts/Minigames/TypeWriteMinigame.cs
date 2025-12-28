using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    public class TypeWriteMinigame : MonoBehaviour
    {
        [SerializeField] private List<InputFieldManager> typingTargets;
        
        private int _currentIndex;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _currentIndex = (_currentIndex + 1) % typingTargets.Count;
                
                typingTargets[_currentIndex].inputField.Select();
            }
        }
    }
}
