using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class InteractToStun : Interactable
    {
        [SerializeField] private StunTrigger stunTrigger;
        [SerializeField] private GameObject vfx;
        [SerializeField] private float stunTime;
        
        [SerializeField] private InteractableTriggerArea triggerArea;
        
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioSfx; 

        private void Awake()
        {
            triggerArea.Init(this);
            vfx.SetActive(false);
        }
        
        public override void Interact()
        {
            if (!triggerArea.IsPlayerInRange) return;
            
            if (audioSource != null && audioSource.isPlaying)
                return;

            StartCoroutine(StartStunningCor());
        }

        private IEnumerator StartStunningCor()
        {
            
            if (audioSource != null && audioSfx != null)
            {
                audioSource.clip = audioSfx;
                audioSource.Play();          
            }
            
            vfx.SetActive(true);
            stunTrigger.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(stunTime);
            
            stunTrigger.gameObject.SetActive(false);
            vfx.SetActive(false);
        }
    }
}