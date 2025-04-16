using UnityEngine;

namespace AllInOne
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LocomotionController _locomotion;
        [SerializeField] private GunAimer _gunAimer;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;
        [Space, Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _movementName;
        [SerializeField] private string _jumpName;
        [SerializeField] private string _fallName;

        [Space, Header("Parameters")]
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;
        [SerializeField] private string _landedName;
        [SerializeField] private string _aimName;

        private int _idleId;
        private int _movementId;
        private int _jumpId;
        private int _fallId;
        
        private int _horizontalId;
        private int _verticalId;
        private int _landedId;
        private int _aimId;

        private Vector2 _inputValue;
        
        private InputController _inputController;

        private bool _isGrounded;
        
        private void Awake()
        {
            _idleId = Animator.StringToHash(_idleName);
            _movementId = Animator.StringToHash(_movementName);
            _jumpId = Animator.StringToHash(_jumpName);
            _fallId = Animator.StringToHash(_fallName);
            
            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);
            _landedId = Animator.StringToHash(_landedName);
            _aimId = Animator.StringToHash(_aimName);

            _locomotion.OnStateChanged += LocomotionStateHandler;
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnMoveInput += MoveHandler;
        }

        private void LocomotionStateHandler(LocomotionState state)
        {
            switch (state)
            {
                case LocomotionState.Idle:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    break;
                case LocomotionState.Movement:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    break;
                case LocomotionState.Jump:
                    _animator.CrossFadeInFixedTime(_jumpId, _crossFadeTime);
                    break;
                case LocomotionState.Fall:
                    _animator.CrossFadeInFixedTime(_fallId, _crossFadeTime);
                    break;
            }
        }

        private void Update()
        {
            _animator.SetFloat(_horizontalId, _inputValue.x, _dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalId, _inputValue.y, _dampTime, Time.deltaTime);

            _animator.SetLayerWeight(1, _gunAimer.AimValue);
            _animator.SetFloat(_aimId, _gunAimer.AimValue);
        }

        private void FixedUpdate()
        {
            /*bool isGrounded = _locomotion.CharacterController.isGrounded;

            if (isGrounded && !_isGrounded)
                _animator.SetTrigger(_landedId);
            _isGrounded = isGrounded;*/
        }

        private void MoveHandler(Vector2 moveInput)
        {
            _inputValue = moveInput;
        }

        private void OnAnimatorMove()
        {
            
        }
    }
}