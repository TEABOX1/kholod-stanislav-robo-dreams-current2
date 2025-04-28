using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AllInOne
{
    public class LobbyInteractor : MonoBehaviour
    {
        public event Action<IInteractable> OnInteract;

        [SerializeField] private Transform _transform;
        [SerializeField] private InputAction _interactAction;
        //[SerializeField] private float _lockDuration;
        [SerializeField] private Money _money;

        private YieldInstruction _lockDelay;

        private IInteractable _currentInteractable;
        private IInteractableService _interactableService;
        private InputController _inputController;

        private readonly HashSet<IInteractable> _interactables = new();

        private void Awake()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();

            _interactAction.Enable();
            _interactAction.performed += InteractHandler;
            _interactableService = ServiceLocator.Instance.GetService<IInteractableService>();

            //_lockDelay = new WaitForSeconds(_lockDuration);
        }

        private void OnDestroy()
        {
            _interactAction.performed -= InteractHandler;
            _interactAction.Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_interactableService.CanInteract(other, out IInteractable interactable))
                return;
            _interactAction.Enable();

            if (interactable.Type == InteractableType.Activate)
            {
                if (_currentInteractable != null)
                    _currentInteractable.Highlight(false);
                _currentInteractable = interactable;
                _currentInteractable.Highlight(true);
                //_interactAction.performed += InteractHandler;
            }
            else
            {
                _interactables.Add(interactable);
                if (_currentInteractable == null)
                {
                    _currentInteractable = interactable;
                    _currentInteractable.Highlight(true);
                }

                if (_currentInteractable != null)
                    _currentInteractable.Highlight(false);
                _currentInteractable = interactable;
                _currentInteractable.Highlight(true);
            }
        }

        private void FixedUpdate()
        {
            IInteractable closest = FindInteractable();
            if (closest != null && closest != _currentInteractable)
            {
                _currentInteractable?.Highlight(false);
                _currentInteractable = closest;
                _currentInteractable.Highlight(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_interactableService.CanInteract(other, out IInteractable interactable))
                return;

            _interactAction.Disable();
            if (interactable.Type == InteractableType.Activate)
            {
                _currentInteractable.Highlight(false);
            }
            else
            {
                _interactables.Remove(interactable);
                if (interactable == _currentInteractable)
                {
                    _currentInteractable.Highlight(false);
                    _currentInteractable = FindInteractable();
                }
            }
        }

        private IInteractable FindInteractable()
        {
            Vector3 center = _transform.position;
            IInteractable closest = null;
            float minSqrDistance = float.MaxValue;
            foreach (IInteractable interactable in _interactables)
            {
                float sqrDistance = (center - interactable.Position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closest = interactable;
                }
            }

            return closest;
        }

        private void InteractHandler(InputAction.CallbackContext context)
        {
            if (_currentInteractable != null)
            {
                if (_currentInteractable.Type == InteractableType.Activate)
                {
                    OnInteract?.Invoke(_currentInteractable);
                    _currentInteractable.Interact();
                }
                else
                {
                    _currentInteractable.onDestroy += InteractableDestroyHandler;
                    _currentInteractable.Interact();
                    OnInteract?.Invoke(_currentInteractable);
                }
            }
        }

        private void InteractableDestroyHandler(IInteractable interactable)
        {
            _interactables.Remove(interactable);
            if (_currentInteractable == interactable)
            {
                _currentInteractable = null;
            }
        }

    }
}