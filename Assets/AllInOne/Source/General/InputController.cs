using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace AllInOne
{
    public class InputController : MonoServiceBase
    {
        public event Action<Vector2> OnMoveInput;
        public event Action<Vector2> OnLookInput;
        public event Action<bool> OnPrimaryInput;
        public event Action<bool> OnSecondaryInput;
        public event Action OnGrenadeInput;
        public event Action<bool> OnScoreInput;
        public event Action OnReload;
        public event Action OnEscape;
        public event Action OnJump;
        public event Action OnInteract;
        public event Action OnHeal;
        public event Action OnInventory;

        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private string _mapName;
        [SerializeField] private string _UImapName;
        [SerializeField] private string _moveName;
        [SerializeField] private string _lookAroundName;
        [SerializeField] private string _pointerPositionName;
        [SerializeField] private string _primaryFireName;
        [SerializeField] private string _secondaryFireName;
        [SerializeField] private string _grenadeName;
        [SerializeField] private string _scoreName;
        [SerializeField] private string _reloadName;
        [SerializeField] private string _escapeName;
        [SerializeField] private string _jumpName;
        [SerializeField] private string _interactName;
        [SerializeField] private string _healName;
        [SerializeField] private string _inventoryName;
        [SerializeField] private CursorLockMode _enabledCursorMode;
        [SerializeField] private CursorLockMode _disabledCursorMode;

        private InputAction _moveAction;
        private InputAction _lookAroundAction;
        private InputAction _pointerPositionAction;
        private InputAction _primaryFireAction;
        private InputAction _secondaryFireAction;
        private InputAction _grenadeAction;
        private InputAction _scoreAction;
        private InputAction _reloadAction;
        private InputAction _escapeAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;
        private InputAction _healAction;
        private InputAction _inventoryAction;

        private InputActionMap _actionMap;
        private InputActionMap _gameplayUIActionMap;

        private bool _initialized;

        public override Type Type { get; } = typeof(InputController);

        public InputAction MoveAction => _moveAction;
        public InputAction LookAroundAction => _lookAroundAction;

        protected override void Awake()
        {
            base.Awake();
            enabled = true;
        }

        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = _enabledCursorMode;

            _inputActionAsset.Enable();

            _actionMap = _inputActionAsset.FindActionMap(_mapName);
            _gameplayUIActionMap = _inputActionAsset.FindActionMap(_UImapName);

            _moveAction = _actionMap[_moveName];
            _lookAroundAction = _actionMap[_lookAroundName];
            //_pointerPositionAction = actionMap[_pointerPositionName];
            _primaryFireAction = _actionMap[_primaryFireName];
            _secondaryFireAction = _actionMap[_secondaryFireName];
            _grenadeAction = _actionMap[_grenadeName];
            _scoreAction = _actionMap[_scoreName];
            _reloadAction = _actionMap[_reloadName];
            _jumpAction = _actionMap[_jumpName];
            _interactAction = _actionMap[_interactName];
            _healAction = _actionMap[_healName];
            _escapeAction = _gameplayUIActionMap[_escapeName];
            _inventoryAction = _gameplayUIActionMap[_inventoryName];

            _moveAction.performed += MovePerformedHandler;
            _moveAction.canceled += MoveCanceledHandler;

            _lookAroundAction.performed += LookPerformedHandler;
            _lookAroundAction.canceled += LookPerformedHandler;

            _primaryFireAction.performed += PrimaryFirePerformedHandler;
            _primaryFireAction.canceled += PrimaryFireCanceledHandler;

            _secondaryFireAction.performed += SecondaryFirePerformedHandler;
            _secondaryFireAction.canceled += SecondaryFireCanceledHandler;

            _grenadeAction.performed += GrenadePerformedHandler;

            _scoreAction.performed += ScorePerformedHandler;
            _scoreAction.canceled += ScoreCanceledHandler;

            _reloadAction.performed += ReloadPerformedHandler;

            _jumpAction.performed += JumpPerformedHandler;

            _interactAction.performed += InteractPerformedHandler;

            _healAction.performed += HealPerformedHandler;

            _escapeAction.performed += EscapePerformedHandler;

            _inventoryAction.performed += InventoryPerformedHandler;

            _initialized = true;
        }

        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = _disabledCursorMode;

            if (_actionMap != null)
                _actionMap.Disable();
        }

        public void DisableEscape()
        {
            _escapeAction.Disable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!_initialized)
                return;

            _moveAction.performed -= MovePerformedHandler;
            _moveAction.canceled -= MoveCanceledHandler;

            _lookAroundAction.performed -= LookPerformedHandler;

            _primaryFireAction.performed -= PrimaryFirePerformedHandler;

            _secondaryFireAction.performed -= SecondaryFirePerformedHandler;
            _secondaryFireAction.canceled -= SecondaryFireCanceledHandler;

            _grenadeAction.performed -= GrenadePerformedHandler;

            _scoreAction.performed -= ScorePerformedHandler;
            _scoreAction.canceled -= ScoreCanceledHandler;

            _reloadAction.performed -= ReloadPerformedHandler;

            _escapeAction.performed -= EscapePerformedHandler;
            _inventoryAction.performed -= InventoryPerformedHandler;

            _jumpAction.performed -= JumpPerformedHandler;
            _interactAction.performed -= InteractPerformedHandler;
            _healAction.performed -= HealPerformedHandler;

            OnMoveInput = null;
            OnLookInput = null;
            OnPrimaryInput = null;
            OnSecondaryInput = null;
            OnGrenadeInput = null;
            OnScoreInput = null;
            OnReload = null;
            OnEscape = null;
            OnJump = null;
        }

        private void MovePerformedHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void MoveCanceledHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void LookPerformedHandler(InputAction.CallbackContext context)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void PrimaryFirePerformedHandler(InputAction.CallbackContext context)
        {
            OnPrimaryInput?.Invoke(true);
        }

        private void PrimaryFireCanceledHandler(InputAction.CallbackContext context)
        {
            OnPrimaryInput?.Invoke(false);
        }

        private void SecondaryFirePerformedHandler(InputAction.CallbackContext context)
        {
            OnSecondaryInput?.Invoke(true);
        }

        private void SecondaryFireCanceledHandler(InputAction.CallbackContext context)
        {
            OnSecondaryInput?.Invoke(false);
        }

        private void GrenadePerformedHandler(InputAction.CallbackContext context)
        {
            OnGrenadeInput?.Invoke();
        }

        private void ScorePerformedHandler(InputAction.CallbackContext context)
        {
            OnScoreInput?.Invoke(true);
        }

        private void ScoreCanceledHandler(InputAction.CallbackContext context)
        {
            OnScoreInput?.Invoke(false);
        }

        private void ReloadPerformedHandler(InputAction.CallbackContext context)
        {
            OnReload?.Invoke();
        }

        private void JumpPerformedHandler(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }

        private void InteractPerformedHandler(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke();
        }

        private void HealPerformedHandler(InputAction.CallbackContext context)
        {
            OnHeal?.Invoke();
        }

        private void EscapePerformedHandler(InputAction.CallbackContext context)
        {
            OnEscape?.Invoke();
        }

        private void InventoryPerformedHandler(InputAction.CallbackContext context)
        {
            OnInventory?.Invoke();
        }
    }
}
