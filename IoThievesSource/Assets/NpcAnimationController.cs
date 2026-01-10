using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NpcAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;    
    [SerializeField] private AnimationClip dance;
    [SerializeField] private bool danceB;
    [SerializeField] private bool moveB;
    [SerializeField] private AnimationClip move;
    void Start()
    {
        animator = GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in clips)
        {
            print(clip.name);
        }
       
    }

    void Update()
    {
            
    }
}
