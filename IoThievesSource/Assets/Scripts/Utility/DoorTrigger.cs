using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum OpenType
{
    OpenA,
    OpenB
}

public class DoorTrigger : MonoBehaviour
{
    [Header("Door Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private OpenType openType;
    
    [Header("Audio")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorOpenSfx; 
    [Header("Mutual Exclusion")]
    [SerializeField] private DoorTrigger otherTrigger;

    private bool _isLocked = false;
    private bool _isAlreadyOpen = false;

    private readonly HashSet<Collider> _occupants = new();

    #region Events

    [HideInInspector] public UnityEvent onTriggerEnterEvent;

    #endregion

    private static bool CanOpenDoor(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanOpenDoor(other))
            return;

        if (other.CompareTag("Player"))
            onTriggerEnterEvent.Invoke();

        if (_isLocked) return;

        _occupants.Add(other);

        if (otherTrigger != null)
            otherTrigger.BlockAudio();

        if (!_isAlreadyOpen)
        {
            PlayDoorOpenSound();
            _isAlreadyOpen = true;
        }

        animator.SetBool(GetBoolParam(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isLocked) return;

        if (!CanOpenDoor(other))
            return;

        _occupants.Remove(other);

        _occupants.RemoveWhere(c => c == null || !c.enabled || !c.gameObject.activeInHierarchy);

        if (_occupants.Count > 0) return;

        if (otherTrigger != null)
            otherTrigger.UnblockAudio();

        _isAlreadyOpen = false;

        animator.SetBool(GetBoolParam(), false);
    }
    
    private bool _audioBlocked = false;
    public void BlockAudio() => _audioBlocked = true;
    public void UnblockAudio() => _audioBlocked = false;
    
    private void PlayDoorOpenSound()
    {
        if (doorAudioSource == null || doorOpenSfx == null || _audioBlocked) return;
        
        doorAudioSource.clip = doorOpenSfx;
        doorAudioSource.Play();
    }

    private string GetBoolParam()
    {
        return openType switch
        {
            OpenType.OpenA => "Open_A",
            OpenType.OpenB => "Open_B",
            _ => ""
        };
    }

    public void SetLocked(bool locked)
    {
        _isLocked = locked;

        if (locked)
            _occupants.Clear();
    }
}
