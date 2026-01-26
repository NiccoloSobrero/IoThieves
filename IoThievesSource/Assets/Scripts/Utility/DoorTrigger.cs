using UnityEngine;
using UnityEngine.Events;

public enum OpenType
{
    OpenA,
    OpenB
}

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private OpenType openType;

    private bool _isLocked = false;

    #region Events

    [HideInInspector] public UnityEvent onTriggerEnterEvent;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy")))
            return;
        
        onTriggerEnterEvent.Invoke();
        
        if (_isLocked) return;
        
        animator.SetBool(GetBoolParam(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isLocked) return;
        
        animator.SetBool(GetBoolParam(), false);
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
    }
}
