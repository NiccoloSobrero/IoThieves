using System;
using AI;
using UnityEngine;

public class NpcAnimationController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    //private static readonly int IsIdle = Animat
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAdapter enemyAdapter;
    private AIController _aiController;
    
    void Start()
    {
        _aiController = GetComponent<AIController>();
        _aiController.OnStateChanged += StateChanged;
        _aiController.BeforeStateChanged += StateAboutToChange;
    }
    
    void StateChanged(AIState state)
    {
        state.OnWalkingVarChanged += SetWalking;
    }
    
    void StateAboutToChange(AIState state)
    {
        state.OnWalkingVarChanged -= SetWalking;
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool(IsWalking, isWalking);
        animator.SetBool("isWalking", isWalking);
    }
}
