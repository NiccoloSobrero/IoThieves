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

        [HideInInspector] public UnityEvent onSendComplaint;
        // [SerializeField] public GameObject errorObj;

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
                    // errorObj.SetActive(true);
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
    }
}