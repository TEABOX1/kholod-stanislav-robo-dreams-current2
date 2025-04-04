using System;
using UnityEngine;

namespace AllInOne
{
    public class GunDamageDealer : MonoBehaviour
    {
        public event Action<int> OnHit;
        
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private HitScanGun _gun;
        //[SerializeField] private int _damage;

        [SerializeField] private WeaponData _data;
        
        public HitScanGun Gun => _gun;
        
        private void Start()
        {
            _gun.OnHit += GunHitHandler;
        }

        private void GunHitHandler(Collider collider)
        {
            if (_healthSystem.GetHealth(collider, out Health health))
                health.TakeDamage(_data.Damage);
            OnHit?.Invoke(health ? 1 : 0);
        }
    }
}