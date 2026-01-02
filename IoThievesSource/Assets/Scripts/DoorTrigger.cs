using UnityEngine;

public enum OpenType
{
    OpenA,
    OpenB
}

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private OpenType openType;

    private bool _isEnabled = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isEnabled) return;
        
        animator.SetBool(GetBoolParam(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isEnabled) return;
        
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

    public void SetEnabled(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }
}
