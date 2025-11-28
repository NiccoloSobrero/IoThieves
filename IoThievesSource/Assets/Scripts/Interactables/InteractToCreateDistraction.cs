using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class InteractToCreateDistraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private float timeToProduce;
        private bool _hasCreatedDistraction;

        private bool _canStartProduction;

        private void Awake()
        {
            _hasCreatedDistraction = false;
            _canStartProduction = true;
        }
        
        public void Interact()
        {
            if (!_canStartProduction || _hasCreatedDistraction) return;
            
            StartCoroutine(ProduceDistraction(timeToProduce));
        }

        private IEnumerator ProduceDistraction(float time)
        {
            _canStartProduction = false;
            
            yield return new WaitForSeconds(time);
            
            _hasCreatedDistraction = true;
            _canStartProduction = true;
        }
    }
}