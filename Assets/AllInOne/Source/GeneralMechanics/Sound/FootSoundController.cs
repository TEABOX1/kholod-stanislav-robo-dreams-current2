using System;
using UnityEngine;

namespace AllInOne
{
    public class FootSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _leftFoot;
        [SerializeField] private AudioSource _rightFoot;
        [SerializeField] private AnimationEventTransmitter _eventTransmitter;
        [SerializeField] private GroundDetector _groundDetector;
        
        private IFootstepSoundService _footstepSoundService;

        private void Start()
        {
            _footstepSoundService = ServiceLocator.Instance.GetService<IFootstepSoundService>();

            _eventTransmitter.onRightFoot += RightFootHandler;
            _eventTransmitter.onLeftFoot += LeftFootHandler;
        }
        
        private void RightFootHandler()
        {
            PlayFootstepSound(_rightFoot);
        }

        private void LeftFootHandler()
        {
            PlayFootstepSound(_leftFoot);
        }

        private void PlayFootstepSound(AudioSource audioSource)
        {
            PhysicMaterial key = _groundDetector.Collider.sharedMaterial;
            audioSource.PlayOneShot(_footstepSoundService.GetFootstepSound(key, transform.position));
        }
    }
}