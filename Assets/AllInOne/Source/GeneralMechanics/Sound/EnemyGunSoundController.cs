using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AllInOne
{
    public class EnemyGunSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _shootSource;
        [SerializeField] private AudioSource _hitSource;
        [SerializeField] private AudioClip[] _shootClips;
        [SerializeField] private AudioClip[] _hitClips;
        [SerializeField] private EnemyHitScanGun _gun;
        [SerializeField] private Transform _muzzle;
        
        private void Start()
        {
            _gun.OnShot += ShotHandler;
            _gun.OnHitPrecise += HitHandler;
        }

        private void ShotHandler()
        {
            _shootSource.transform.position = _muzzle.position;
            _shootSource.PlayOneShot(_shootClips[Random.Range(0, _shootClips.Length)]);
        }

        private void HitHandler(RaycastHit hit)
        {
            //Debug.Break();
            _hitSource.transform.position = hit.point;
            _hitSource.PlayOneShot(_hitClips[Random.Range(0, _hitClips.Length)]);
        }
    }
}