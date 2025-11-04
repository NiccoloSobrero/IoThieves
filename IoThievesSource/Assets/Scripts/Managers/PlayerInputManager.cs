using Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInputActions _actions;

        public Vector2 Move { get; private set; }

        private void Awake()
        {
            _actions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _actions.Player.Enable();
        
            _actions.Player.Move.performed += OnMove;
            _actions.Player.Move.canceled += OnStopMove;
        
            _actions.Player.Click.performed += OnPointerClick;
        }

        private void OnDisable()
        {
            _actions.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void OnStopMove(InputAction.CallbackContext context)
        {
            Move = Vector2.zero;
        }

        private void OnPointerClick(InputAction.CallbackContext context)
        {
            // Fire event
            EventsManager.OnPointerClick?.Invoke(Input.mousePosition);
        }
    }
}
