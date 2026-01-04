using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class InteractToStun : Interactable
    {
        [SerializeField] private StunTrigger stunTrigger;
        [SerializeField] private ParticleSystem smokeVFX;
        [SerializeField] private float stunTime;
        
        [SerializeField] private InteractableTriggerArea triggerArea;

        private void Awake()
        {
            triggerArea.Init(this);
        }
        
        public override void Interact()
        {
            if (!triggerArea.IsPlayerInRange) return;

            StartCoroutine(StartStunningCor());
        }

        private IEnumerator StartStunningCor()
        {
            smokeVFX.Play();
            stunTrigger.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(stunTime);
            
            stunTrigger.gameObject.SetActive(false);
            smokeVFX.Stop();
        }
    }
}