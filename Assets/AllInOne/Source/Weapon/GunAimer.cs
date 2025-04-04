using System;
using Cinemachine;
using UnityEngine;

namespace AllInOne
{
    public class GunAimer : MonoBehaviour
    {
        [SerializeField] private Transform _gunRegularAnchor;
        [SerializeField] private Transform _gunAimAnchor;
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;
        [SerializeField] private CinemachineMixingCamera _mixingCamera;
        [SerializeField] private float _aimSpeed;
        [SerializeField] private float _regularSensetivity;
        [SerializeField] private float _aimSensetivity;
        [SerializeField] private CameraController _cameraController;

        private Vector3 _hitPoint;
        private Vector3 _collisonPoint;
        private bool _hasCameraHit;
        private float _aimValue;
        private float _targetAimValue;

        public Vector3 AimPoint => _hitPoint;
        public Vector3 CollisionPoint => _collisonPoint;

        private InputController _inputController;

        private void OnEnable()
        {
            if (_inputController == null)
                _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnSecondaryInput += SecondaryInputHandler;
            _cameraController.SetSensitivity(_regularSensetivity);
        }

        private void OnDisable()
        {
            _inputController.OnSecondaryInput -= SecondaryInputHandler;
        }

        private void FixedUpdate()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            _collisonPoint = _hitPoint;
            _hasCameraHit = false;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore))
            {
                _hasCameraHit = true;
                _hitPoint = hitInfo.point;
            }

            _gunTransform.LookAt(_hitPoint);
            
            if (Physics.Raycast(_gunTransform.position, _gunTransform.forward, out RaycastHit collisionInfo, _rayDistance,
                    _rayMask, QueryTriggerInteraction.Ignore))
            {
                _collisonPoint = collisionInfo.point;
            }
        }

        private void Update()
        {
            _aimValue = Mathf.MoveTowards(_aimValue, _targetAimValue, _aimSpeed * Time.deltaTime);
            _mixingCamera.m_Weight0 = 1f - _aimValue;
            _mixingCamera.m_Weight1 = _aimValue;
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _hasCameraHit ? new Color(1f, 0.5f, 0f, 1f) : Color.red;
            Gizmos.DrawLine(_gunTransform.position, _hitPoint);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_cameraTransform.position, _hitPoint);
        }

        private void SecondaryInputHandler(bool performed)
        {
            _targetAimValue = performed ? 1f : 0f;
            _cameraController.SetSensitivity(performed ? _aimSensetivity : _regularSensetivity);
        }
    }
}