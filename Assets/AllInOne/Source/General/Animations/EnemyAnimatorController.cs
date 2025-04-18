using UnityEngine;

namespace AllInOne
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyController _enemy;
        [SerializeField] private EnemyHitScanGun _hitScanGun;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _dampTime;
        [Space, Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _movementName;
        [SerializeField] private string _deathName;
        [SerializeField] private string _meeleName;

        [Space, Header("Parameters")]
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;
        [SerializeField] private string _aimName;

        private int _idleId;
        private int _movementId;
        private int _deathId;
        private int _meeleId;

        private Vector2 _movementValue;
        
        private int _horizontalId;
        private int _verticalId;
        private int _aimId;

        private float _aimValue;
        
        private void Awake()
        {
            _idleId = Animator.StringToHash(_idleName);
            _movementId = Animator.StringToHash(_movementName);
            _deathId = Animator.StringToHash(_deathName);
            _meeleId = Animator.StringToHash(_meeleName);

            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);
            _aimId = Animator.StringToHash(_aimName);

            _enemy.onBehaviourChanged += BehaviourStateHandler;
            _hitScanGun.OnMeeleHit += MeeleHit;
        }

        private void BehaviourStateHandler(EnemyBehaviour state)
        {
            switch (state)
            {
                case EnemyBehaviour.Idle:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    _aimValue = 0f;
                    break;
                case EnemyBehaviour.Patrol:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    _aimValue = 0f;
                    break;
                case EnemyBehaviour.Search:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    _aimValue = 0f;
                    break;
                case EnemyBehaviour.Shoot:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    _aimValue = 1f;
                    break;
                case EnemyBehaviour.Attack:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    _aimValue = 1f;
                    break;
                case EnemyBehaviour.Death:
                    _animator.CrossFadeInFixedTime(_deathId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    _aimValue = 0f;
                    break;
            }
        }

        private void MeeleHit(Collider collider)
        {
            _animator.CrossFadeInFixedTime(_meeleId, _crossFadeTime);
        }

        private void Update()
        {
            _animator.SetFloat(_horizontalId, _movementValue.x, _dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalId, _movementValue.y, _dampTime, Time.deltaTime);
            
            _animator.SetLayerWeight(1, _aimValue);
            _animator.SetFloat(_aimId, _aimValue);
        }

        private void OnAnimatorMove()
        {
            
        }
    }
}