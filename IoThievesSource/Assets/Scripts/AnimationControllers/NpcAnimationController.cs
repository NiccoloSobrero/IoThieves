using System;
using AI;
using UnityEngine;

namespace AnimationControllers
{
    public class NpcAnimationController : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsStunned = Animator.StringToHash("isStunned");
        //private static readonly int IsIdle = Animat
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyAdapter enemyAdapter;
        private AIController _aiController;
    
        public void Initialize()
        {
            Debug.Log(gameObject.name + " Initialize ANIMACONTROLLER");
            _aiController = enemyAdapter.AIController;
            _aiController.OnStateChanged += StateChanged;
            _aiController.BeforeStateChanged += StateAboutToChange;
        }
    
        void StateChanged(AIState state)
        {
            if(state is not null)
                state.OnWalkingVarChanged += SetWalking;
        }
    
        void StateAboutToChange(AIState state)
        {
            if (state is not null)
                state.OnWalkingVarChanged -= SetWalking;
        }

        public void SetWalking(bool value)
        {
            animator.SetBool(IsWalking, value);
        }

        public void SetStunned(bool value)
        {
            animator.SetBool(IsStunned, value); 
        }

        private void OnDisable()
        {
            SetWalking(false);
            SetStunned(false);
        }

        private void OnEnable()
        {
            Debug.Log("ENABLED CONTROLLER OF " + gameObject.name);
        }
    }
}
