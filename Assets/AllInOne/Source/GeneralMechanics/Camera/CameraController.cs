﻿using UnityEngine;

namespace AllInOne
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _pitchAnchor;
        [SerializeField] private Transform _yawAnchor;
        [SerializeField] private float _sensitivity;

        private float _pitch = 20f;
        private float _yaw = 0f;

        private Vector2 _lookInput;

        private void Start()
        {
            ServiceLocator.Instance.GetService<InputController>().OnLookInput += LookHandler;
        }

        private void LateUpdate()
        {
            _pitch -= _lookInput.y * _sensitivity * Time.deltaTime;
            _yaw += _lookInput.x * _sensitivity * Time.deltaTime;

            _pitch = Mathf.Clamp(_pitch, -40f, 40f);

            _pitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            _yawAnchor.rotation = Quaternion.Euler(0f, _yaw, 0f);
        }

        private void LookHandler(Vector2 lookInput)
        {
            _lookInput = lookInput;
        }

        public void SetSensitivity(float sensitivity)
        {
            _sensitivity = sensitivity;
        }
    }
}