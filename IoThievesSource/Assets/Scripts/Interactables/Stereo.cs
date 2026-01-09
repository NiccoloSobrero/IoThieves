using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Interactables
{
    public class Stereo : Interactable 
    {
        [SerializeField] private bool isPlayingMusic = false;
        private float elapsedTime;
        private float TotalSongTime = 15.0f;
        public bool IsPlayingMusic { get => isPlayingMusic;}
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
            while (isPlayingMusic)
            {
                elapsedTime += Time.deltaTime;
            
                if (elapsedTime >= TotalSongTime)
                {
                    isPlayingMusic = false;    
                }

                yield return null;
            }
        }
    }
}