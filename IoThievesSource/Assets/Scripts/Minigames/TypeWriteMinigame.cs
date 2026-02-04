using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames
{
    public class TypeWriteMinigame : MonoBehaviour
    {
        [SerializeField] private List<TypeWriteSection> typingSections;
        
        private int _currentIndex;
        
        private TypeWriteSection _lastSelectedSection;

        [HideInInspector] public UnityEvent onSendComplaint;

        public void Init()
        {
            foreach (TypeWriteSection typeWriteSection in typingSections)
            {
                typeWriteSection.Init(this);
            }
        }

        public void Send()
        {
            // Check correctness
            foreach (var section in typingSections)
            {
                if (section.HasTooManyErrors())
                {
                    Debug.Log("The complaint has an error and cannot be sent.");
                    return;
                }
            }
            
            // Invoke event
            onSendComplaint.Invoke();
            
            gameObject.SetActive(false);
        }
        
        public IEnumerator NextSection()
        {
            yield return new WaitForEndOfFrame();
            
            typingSections[_currentIndex].OnDeselect(null);
           
            if (_currentIndex + 1 < typingSections.Count)
            {
                _currentIndex++;
                typingSections[_currentIndex].OnSelect(null);
            }
        }

        public void SelectSection(TypeWriteSection section)
        {
            if (_lastSelectedSection != null && _lastSelectedSection != section)
            {
                _lastSelectedSection.OnDeselect(null);
            }

            for (int i = 0; i < typingSections.Count; i++)
            {
                if (typingSections[i] == section)
                {
                    _currentIndex = i;
                    break;
                }
            }
            
            _lastSelectedSection = section;
        }
    }
}