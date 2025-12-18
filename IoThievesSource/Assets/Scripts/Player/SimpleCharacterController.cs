using System.Collections;
using Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleCharacterController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform graphics;
        [SerializeField] private Animator characterAnimator;
        
        [Header("Settings")] 
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        private Camera _camera;
        private CharacterController _controller;

        private Vector3 _forward;
        private Vector3 _right;
        
        private float _turnSmoothVelocity;
        
        private const float DistanceThreshold = 0.1f;
        
        private void Awake()
        {
            _camera = Camera.main;
            _controller = GetComponent<CharacterController>();

            if (!_camera) return;
            
            // Works under assumption that camera doesn't turn
            _forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        
            _right = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 moveDir = _forward * PlayerInputManager.Instance.Move.y 
                            + _right * PlayerInputManager.Instance.Move.x;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                moveDir = moveDir.normalized;
                
                _controller.Move(moveDir * (moveSpeed * Time.deltaTime));
                
                characterAnimator.SetBool("IsMoving", true);
            
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(graphics.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                graphics.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                characterAnimator.SetBool("IsMoving", false);
            }
        }

        public void MoveTowards(Vector3 targetPosition)
        {
            StartCoroutine(MoveTowardsRoutine(targetPosition));
        }
        
        private IEnumerator MoveTowardsRoutine(Vector3 targetPosition)
        {
            while (Vector3.Distance(targetPosition, transform.position) > DistanceThreshold)
            {
                Vector3 moveDir = targetPosition - transform.position;

                moveDir = moveDir.normalized;
                
                _controller.Move(moveDir * (moveSpeed * Time.deltaTime));
            
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(graphics.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                graphics.rotation = Quaternion.Euler(0f, angle, 0f);

                yield return null;
            }
        }
    }
}
