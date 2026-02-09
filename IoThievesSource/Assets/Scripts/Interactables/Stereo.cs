using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Interactables
{
    public class Stereo : Interactable
    {
        [SerializeField] private bool isPlayingMusic = false;
        [SerializeField] private InteractableTriggerArea triggerArea;
        private float elapsedTime;
        private float TotalSongTime = 16.0f;

        [SerializeField] private GameObject musicVFX;
        
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioSfx; 
        
        public bool IsPlayingMusic { get => isPlayingMusic;}
        
        private void Awake()
        {
            triggerArea.Init(this);
        }
        
        public override void Interact()
        {
            if (!isPlayingMusic)
            {
               isPlayingMusic = true;
               elapsedTime = 0;
               StartCoroutine(Music());
            }
        }
        
        private IEnumerator Music()
        {
            
            if (audioSource != null && audioSfx != null)
            {
                audioSource.clip = audioSfx;
                audioSource.Play();          
            }
            musicVFX.SetActive(true);
            
            while (isPlayingMusic)
            {
                elapsedTime += Time.deltaTime;
            
                if (elapsedTime >= TotalSongTime)
                {
                    isPlayingMusic = false;    
                    musicVFX.SetActive(false);
                }

                yield return null;
            }
        }
    }
}