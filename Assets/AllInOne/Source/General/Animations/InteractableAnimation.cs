using System.Collections;
using UnityEngine;

namespace AllInOne
{
    public class InteractableAnimation : MonoBehaviour
    {
        [SerializeField] private LocomotionController _locomotionController;
        [SerializeField] private LobbyInteractor _interactor;
        [SerializeField] private Animator _animator;
        [SerializeField] private HandsIK _handsIK;
        [SerializeField] private string _pickupName;
        [SerializeField] private string _activateName;
        [SerializeField] private string _idleName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _lockDuration;

        private int _pickupId;
        private int _activateId;
        private int _idleId;

        private YieldInstruction _lockDelay;
        private IInteractable _interactable;

        private InputController _inputController;
        private IHealth _compositeHealth;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();

            _compositeHealth = ServiceLocator.Instance.GetService<IPlayerService>().Player
                .GetComponent<IHealth>();

            _compositeHealth.OnDeath += PlayerDeathHandler;
            
            _lockDelay = new WaitForSeconds(_lockDuration);
            
            _pickupId = Animator.StringToHash(_pickupName);
            _activateId = Animator.StringToHash(_activateName);
            _idleId = Animator.StringToHash(_idleName);

            _interactor.OnInteract += InteractHandler;
        }

        private void InteractHandler(IInteractable interactable)
        {
            _interactable = interactable;
            StartCoroutine(LockRoutine());
        }

        private IEnumerator LockRoutine()
        {
            //_inputController.Lock();
            if (_locomotionController.LocomotionState != LocomotionState.Idle)
            {
                _locomotionController.OnStateChanged += LocomotionStateHandler;
            }
            else
            {
                PlayInteract();
            }
            
            yield return _lockDelay;
            //_inputController.Unlock();
            _handsIK.EnableIK();
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
        }

        private void LocomotionStateHandler(LocomotionState state)
        {
            _locomotionController.OnStateChanged -= LocomotionStateHandler;
            PlayInteract();
        }

        private void PlayInteract()
        {
            int animationId = _interactable.Type switch
            {
                InteractableType.PickUp => _pickupId,
                InteractableType.Activate => _activateId,
                _ => _activateId
            };
            _animator.CrossFadeInFixedTime(animationId, _crossFadeTime);
            _handsIK.DisableIK();
            _interactable = null;
        }

        private void PlayerDeathHandler()
        {
            StopAllCoroutines();
        }
    }
}