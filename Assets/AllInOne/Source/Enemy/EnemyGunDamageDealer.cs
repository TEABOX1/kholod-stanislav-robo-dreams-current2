using System;
using UnityEngine;

namespace AllInOne
{
    public class EnemyGunDamageDealer : MonoBehaviour
    {
        public event Action<int> OnHit;
        
        [SerializeField] private EnemyHitScanGun _gun;
        //[SerializeField] private int _damage;

        [SerializeField] private WeaponData _data;
        
        public EnemyHitScanGun Gun => _gun;
        
        private IHealthService _healthService;
        
        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            
            _gun.OnHit += GunHitHandler;
            _gun.OnMeeleHit += MeeleHitHandler;
        }

        private void GunHitHandler(Collider collider)
        {
            if (_healthService.GetHealth(collider, out Health health))
                health.TakeDamage(_data.Damage);
            OnHit?.Invoke(health ? 1 : 0);
        }

        private void MeeleHitHandler(Collider collider)
        {
            if (_healthService.GetHealth(collider, out Health health))
                health.TakeDamage(10);
            OnHit?.Invoke(health ? 1 : 0);
        }
    }
}