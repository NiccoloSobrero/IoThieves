using AI;
using UnityEngine;

namespace AnimationControllers
{
    public class NpcAnimationController : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsStunned = Animator.StringToHash("isStunned");
        private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");
        
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyAdapter enemyAdapter;
        private AIController _aiController;
        public void Initialize()
        {
            _aiController = enemyAdapter.AIController;
            SetWalking(false);
            SetStunned(false);
            SetInteracting(false);
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
            {
                Reset(); 
                state.OnWalkingVarChanged -= SetWalking;
            }
        }

        public void SetWalking(bool value)
        {
            animator.SetBool(IsWalking, value);
        }

        public void SetStunned(bool value)
        {
            animator.SetBool(IsStunned, value); 
        }

        public void SetInteracting(bool value)
        {
            animator.SetBool(IsInteracting, value);
        }

        private void OnDisable()
        {
            SetWalking(false);
            SetStunned(false);
            SetInteracting(false); 
        }

        private void Reset()
        {
            SetWalking(false);
            SetStunned(false);
            SetInteracting(false); 
        }
    }
}
