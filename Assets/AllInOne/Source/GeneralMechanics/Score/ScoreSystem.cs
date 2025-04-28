using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class ScoreSystem : MonoBehaviour
    {
        public event Action OnDataUdpated;
        
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private GunDamageDealer _gunDamageDealer;
        [SerializeField] private GrenadeDamageDealer _grenadeDamageDealer;
        
        private Vector3Int _kda;
        private int _shotCount;
        private int _hitCount;
        private ISaveService _saveService;

        public Vector3Int KDA => _kda;
        public int Accuracy => _shotCount == 0f ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);

        private void Start()
        {
            _gunDamageDealer.OnHit += HitHandler;
            _gunDamageDealer.Gun.OnShot += ShotHandler;
            _healthSystem.OnCharacterDeath += CharacterDeathHandler;
            _grenadeDamageDealer.GrenadeAction.OnGrenadeSpawned += GrenadeSpawnHandler;
            _grenadeDamageDealer.OnHit += HitHandler;

            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            _kda = new Vector3Int(_saveService.SaveData.kDASavedata.kill
                                , _saveService.SaveData.kDASavedata.death
                                , _saveService.SaveData.kDASavedata.assist);
            _shotCount = _saveService.SaveData.kDASavedata.accuracy;
            OnDataUdpated?.Invoke();
        }

        private void HitHandler(int hits)
        {
            _hitCount += hits;
            SaveAccuracy();
            OnDataUdpated?.Invoke();
        }

        private void ShotHandler()
        {
            _shotCount++;
            SaveAccuracy();
            OnDataUdpated?.Invoke();
        }

        private void CharacterDeathHandler(Health health)
        {
            _kda.x++;
            _saveService.SaveData.kDASavedata.kill = _kda.x;
            _saveService.SaveAll();
            OnDataUdpated?.Invoke();
        }

        private void GrenadeSpawnHandler(Grenade grenade)
        {
            _shotCount++;
            SaveAccuracy();
            OnDataUdpated?.Invoke();
        }

        private void SaveAccuracy()
        {
            _saveService.SaveData.kDASavedata.accuracy = _shotCount == 0f ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);
            _saveService.SaveAll();
        }
    }
}