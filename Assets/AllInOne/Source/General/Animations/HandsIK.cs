using System;
using UnityEngine;

namespace AllInOne
{
    public class HandsIK : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GunAimer _gunAimer;
        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _leftHand;
        [SerializeField, Range(0f, 1f)] private float _handsFactor;
        [SerializeField, Range(0f, 1f)] private float _lookAtFactor; 
        [SerializeField, Range(0f, 1f)] private float _bodyWeight;
        [SerializeField, Range(0f, 1f)] private float _headWeight;
        [SerializeField, Range(0f, 1f)] private float _eyeWeight;
        [SerializeField, Range(0f, 1f)] private float _lookAtClamp;
        [SerializeField] private GameObject _logicalWeapon;
        [SerializeField] private GameObject _animatedWeapon;
        
        private float _regularLookAtFactor;
        private float _regularHandsFactor;

        private void Start()
        {
            _logicalWeapon.SetActive(true);
            _animatedWeapon.SetActive(false);
            
            _regularLookAtFactor = _lookAtFactor;
            _regularHandsFactor = _handsFactor;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            _animator.SetLookAtPosition(_gunAimer.AimPoint);
            _animator.SetLookAtWeight(_lookAtFactor, _bodyWeight, _headWeight, _eyeWeight, _lookAtClamp);
            
            _rightHand.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            SetHandIK(AvatarIKGoal.RightHand, position, rotation);
            
            _leftHand.GetPositionAndRotation(out position, out rotation);
            SetHandIK(AvatarIKGoal.LeftHand, position, rotation);
        }

        private void SetHandIK(AvatarIKGoal goal, Vector3 position, Quaternion rotation)
        {
            _animator.SetIKPosition(goal, position);
            _animator.SetIKRotation(goal, rotation);
            _animator.SetIKPositionWeight(goal, _handsFactor);
            _animator.SetIKRotationWeight(goal, _handsFactor);
        }

        public void DisableIK()
        {
            _handsFactor = 0f;
            _lookAtFactor = 0f;
            
            _logicalWeapon.SetActive(false);
            _animatedWeapon.SetActive(true);
        }

        public void EnableIK()
        {
            _handsFactor = _regularHandsFactor;
            _lookAtFactor = _regularLookAtFactor;
            
            _logicalWeapon.SetActive(true);
            _animatedWeapon.SetActive(false);
        }
    }
}