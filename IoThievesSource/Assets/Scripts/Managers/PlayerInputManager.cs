using DialogueSystem;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputContext
{
    None,
    Gameplay,
    Minigame,
    Dialogue,
    Disabled,
    LevelSelection
}

namespace Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        
        private PlayerInputActions _actions;
        private InputContext _currentContext = InputContext.None;
        private bool _isInitialized = false;
        public InputContext CurrentContext => _currentContext;

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
                return;
            }
            _isInitialized = true;
            
            _actions = new PlayerInputActions();
        }

        private void Start()
        {
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
            _actions.LevelSelection.Enable();
        
            // Gameplay map
            _actions.Gameplay.Move.performed += OnMove;
            _actions.Gameplay.Move.canceled += OnStopMove;
        
            _actions.Gameplay.Click.performed += OnPointerClick;
            
            _actions.Gameplay.Throw.performed += ThrowDistraction;
            _actions.Gameplay.IncreaseThrowDistance.performed += IncreaseThrowDistance;
            _actions.Gameplay.DecreaseThrowDistance.performed += DecreaseThrowDistance;

            _actions.Gameplay.Pause.performed += PauseGame;
            
            // Minigames map
            
            
            // Dialogue map
            _actions.Dialogue.ContinueDialogue.performed += ContinueDialogue;
            
            // Level selection
            _actions.LevelSelection.Next.performed += NextLevel;
            _actions.LevelSelection.Previous.performed += PreviousLevel;
            _actions.LevelSelection.Play.performed += PlayLevel;
            _actions.LevelSelection.Exit.performed += ExitLevelSelection;
            
            SetContext(InputContext.Gameplay);
            EventsManager.OnOptionsSave.AddListener(LoadUserPreferences);
        }

        private void OnDisable()
        {
            if (!_isInitialized) return;
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
                case InputContext.LevelSelection:
                    _actions.LevelSelection.Enable();
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
            _actions.LevelSelection.Disable();
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
        
        private void ThrowDistraction(InputAction.CallbackContext context)
        {
            EventsManager.OnThrow.Invoke();
        }

        private void IncreaseThrowDistance(InputAction.CallbackContext context)
        {
            EventsManager.OnIncreaseThrowDistance.Invoke();
        }
        
        private void DecreaseThrowDistance(InputAction.CallbackContext context)
        {
            EventsManager.OnDecreaseThrowDistance.Invoke();
        }
        
        private void ContinueDialogue(InputAction.CallbackContext context)
        {
            DialogueManager.Instance.GenerateNextLine();
        }

        private void PauseGame(InputAction.CallbackContext context)
        {
            EventsManager.OnPauseGame?.Invoke();
        }

        private void PreviousLevel(InputAction.CallbackContext context)
        {
            GameManager.Instance.LevelSelectorManager.PreviousLevel();
        }
        
        private void NextLevel(InputAction.CallbackContext context)
        {
            GameManager.Instance.LevelSelectorManager.NextLevel();
        }

        private void PlayLevel(InputAction.CallbackContext context)
        {
            EventsManager.OnPlayLevel.Invoke(GameManager.Instance.LevelSelectorManager.CurrentSelectedLevel);
        }

        private void ExitLevelSelection(InputAction.CallbackContext context)
        {
            GameManager.Instance.LevelSelectorManager.BackToMainMenu();
        }
    }
}
