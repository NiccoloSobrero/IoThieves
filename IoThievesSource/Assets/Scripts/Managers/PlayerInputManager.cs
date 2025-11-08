using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputContext
{
    Gameplay,
    Minigame
}

namespace Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        
        private PlayerInputActions _actions;
        private InputContext _currentContext;

        public Vector2 Move { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _actions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _actions.Gameplay.Enable();
            _actions.Minigame.Enable();
        
            _actions.Gameplay.Move.performed += OnMove;
            _actions.Gameplay.Move.canceled += OnStopMove;
        
            _actions.Gameplay.Click.performed += OnPointerClick;
            
            SetContext(InputContext.Gameplay);
        }

        private void OnDisable()
        {
            _actions.Gameplay.Disable();
        }

        public void SetContext(InputContext context)
        {
            if (_currentContext ==  context) return;
            
            DisableAll();
            _currentContext = context;

            switch (context)
            {
                case InputContext.Gameplay:
                    _actions.Gameplay.Enable();
                    break;
                case InputContext.Minigame:
                    _actions.Minigame.Enable();
                    break;
                default:
                    Debug.LogError($"Unhandled context {context}");
                    break;
            }
        }

        private void DisableAll()
        {
            _actions.Gameplay.Disable();
            _actions.Minigame.Disable();
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
            EventsManager.OnPointerClickInWorld?.Invoke(Input.mousePosition);
        }

        private void OnRightClick(InputAction.CallbackContext context)
        {
            MinigamesManager.Instance.CheckCompletion();
        }
    }
}
