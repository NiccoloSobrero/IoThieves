using AI;
using UnityEngine;

public class AttractedByStereo_AnimationController : MonoBehaviour
{
    private static readonly int isWalking = Animator.StringToHash("isWalking");
    private static readonly int isDancing = Animator.StringToHash("isDancing");
    
    [SerializeField] private Animator _animatorController;

    [SerializeField] private AttractedByStereo sourceScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sourceScript.OnMovementVarChanged += SetWalkingVariable;
        sourceScript.OnDancingVarChanged += SetDancingVariable;
    }

    private void SetWalkingVariable(bool value)
    {
       _animatorController.SetBool(isWalking, value); 
    }
    
    private void SetDancingVariable(bool value)
    {
        _animatorController.SetBool(isDancing, value); 
    }
}
