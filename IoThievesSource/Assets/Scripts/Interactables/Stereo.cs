using NUnit.Framework.Constraints;
using UnityEngine;

namespace Interactables
{
    public class Stereo : MonoBehaviour
    {
        [SerializeField] private bool isPlayingMusic = false;
        
        public bool IsPlayingMusic { get => isPlayingMusic;}
    }
}