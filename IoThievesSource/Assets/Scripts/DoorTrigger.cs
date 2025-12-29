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

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool(GetBoolParam(), true);
    }

    private void OnTriggerExit(Collider other)
    {
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
}
