using DialogueSystem;
using Events;
using Interactables.ThrowableDistraction;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputContext
{
    None,
    Gameplay,
    Minigame,
    Dialogue,
    Disabled
}

namespace Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        
        private PlayerInputActions _actions;
        private InputContext _currentContext = InputContext.None;
        public InputContext CurrentContext => _currentContext;

        [Header("References")]
        [SerializeField] private PlayerThrower playerThrower;

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
            LoadUserPreferences();
        }

        private void LoadUserPreferences()
        {
            string userPreferences = GameManager.Instance.LoadUserInputPreferences();
            _actions.LoadBindingOverridesFromJson(userPreferences);
        }

        private void OnEnable()
        {
            _actions.Gameplay.Enable();
            _actions.Minigame.Enable();
            _actions.Dialogue.Enable();
            _actions.Disabled.Enable();
        
            // Gameplay map
            _actions.Gameplay.Move.performed += OnMove;
            _actions.Gameplay.Move.canceled += OnStopMove;
        
            _actions.Gameplay.Click.performed += OnPointerClick;
            
            _actions.Gameplay.Throw.performed += ThrowDistraction;
            _actions.Gameplay.IncreaseThrowDistance.performed += IncreaseThrowDistance;
            _actions.Gameplay.DecreaseThrowDistance.performed += DecreaseThrowDistance;

            _actions.Gameplay.Pause.performed += PauseGame;
            
            // Minigames map
            _actions.Minigame.Click.canceled += OnMouseRelease;
            
            // Dialogue map
            _actions.Dialogue.ContinueDialogue.performed += ContinueDialogue;
            
            SetContext(InputContext.Gameplay);
            EventsManager.OnOptionsSave.AddListener(LoadUserPreferences);
        }

        private void OnDisable()
        {
            DisableAll();
            EventsManager.OnOptionsSave.RemoveListener(LoadUserPreferences);
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
                case InputContext.Dialogue:
                    _actions.Dialogue.Enable();
                    break;
                case InputContext.Disabled:
                    _actions.Disabled.Enable();
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
            _actions.Dialogue.Disable();
            _actions.Disabled.Disable();
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

        private void OnMouseRelease(InputAction.CallbackContext context)
        {
            // MinigamesManager.Instance.CheckCompletion();
        }

        private void ThrowDistraction(InputAction.CallbackContext context)
        {
            playerThrower.Throw();
        }

        private void IncreaseThrowDistance(InputAction.CallbackContext context)
        {
            playerThrower.IncreaseThrowDistance();
        }
        
        private void DecreaseThrowDistance(InputAction.CallbackContext context)
        {
            playerThrower.DecreaseThrowDistance();
        }
        
        private void ContinueDialogue(InputAction.CallbackContext context)
        {
            DialogueManager.Instance.GenerateNextLine();
        }

        private void PauseGame(InputAction.CallbackContext context)
        {
            EventsManager.OnPauseGame?.Invoke();
        }
    }
}
