using Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleCharacterController : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private PlayerInputManager inputManager;

        [Header("Settings")] 
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        private Camera _camera;
        private CharacterController _controller;

        private Vector3 _forward;
        private Vector3 _right;
        private void Awake()
        {
            _camera = Camera.main;
            _controller = GetComponent<CharacterController>();

            if (!_camera) return;
            
            _forward = _camera.transform.forward;
            _forward.y = 0;
            _forward.Normalize();
        
            _right = _camera.transform.right;
            _right.y = 0;
            _right.Normalize();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 move = (_forward * inputManager.Move.y + _right * inputManager.Move.x).normalized * (moveSpeed * Time.deltaTime);
            _controller.Move(move);
        }
    }
}
