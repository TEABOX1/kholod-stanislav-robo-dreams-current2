using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AllInOne
{
    /// <summary>
    /// Script that subscribes to events of CompositeHealth in order to play sounds
    /// when health takes damage and dies
    /// </summary>
    public class HealthSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _healthSource;
        [SerializeField] private AudioClip[] _hitSounds;
        [SerializeField] private AudioClip[] _deathSounds;
        [SerializeField] private Health _compositeHealth;

        private void Start()
        {
            _compositeHealth.OnTakeDamage += DamageHandler;
            _compositeHealth.OnDeath += DeathHandler;
        }

        private void DamageHandler(int damage)
        {
            _healthSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Length)]);
        }

        private void DeathHandler()
        {
            _healthSource.PlayOneShot(_deathSounds[Random.Range(0, _deathSounds.Length)]);
        }
    }
}