using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames
{
    public class TypeWriteMinigame : MonoBehaviour
    {
        [SerializeField] private List<TypeWriteSection> typingSections;
        
        private int _currentIndex;

        [HideInInspector] public UnityEvent onSendComplaint;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                typingSections[_currentIndex].OnDeselect(null);
                
                _currentIndex = (_currentIndex + 1) % typingSections.Count;
                
                typingSections[_currentIndex].OnSelect(null);
            }
        }

        public void Init()
        {
            foreach (TypeWriteSection typeWriteSection in typingSections)
            {
                typeWriteSection.Init();
            }
        }

        public void Send()
        {
            // Check correctness
            foreach (var section in typingSections)
            {
                if (section.HasAnError())
                {
                    Debug.Log("The complaint has an error and cannot be sent.");
                    return;
                }
            }
            
            // Invoke event
            onSendComplaint.Invoke();
        }
    }
}