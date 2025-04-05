using System;
using System.Collections;
using UnityEngine;

namespace AllInOne
{
    public class HitScanGunCooldown : HitScanGun
    {
        public event Action<int> OnChargeChanged;
        public event Action<bool> OnReload;
        
        /*[SerializeField] private float _cooldownTime;
        [SerializeField] private int _maxCharge;
        [SerializeField] private int _chargePerShot;
        [SerializeField] private float _reloadTime;*/

        [SerializeField] private WeaponData _data;
        
        private Cooldown _cooldown;
        private Cooldown _reload;

        private int _currentCharge;

        public int MaxCharge => _data.MaxCharge;
        public int CurrentCharge => _currentCharge;
        
        public Cooldown Cooldown => _cooldown;
        public Cooldown Reload => _reload;
        
        private void Awake()
        {
            _cooldown = new Cooldown(_data.CooldownTime);
            _reload = new Cooldown(_data.ReloadTime);
            _currentCharge = _data.MaxCharge;
        }
        
        protected override void Start()
        {
            _inputController.OnReload += ReloadHandler;
            
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void PrimaryInputHandler(bool isHold)
        {
            if (_cooldown.IsOnCooldown || _reload.IsOnCooldown)
                return;
            base.PrimaryInputHandler(isHold);
            if (isHold)
                return;
            StartCoroutine(_cooldown.Begin());
            _currentCharge -= _data.ChargePerShot;
            if (_currentCharge <= 0)
            {
                _currentCharge = 0;
                StartCoroutine(ReloadRoutine());
            }
            OnChargeChanged?.Invoke(_currentCharge);
        }

        private IEnumerator ReloadRoutine()
        {
            OnReload?.Invoke(true);
            yield return _reload.Begin();
            _currentCharge = _data.MaxCharge;
            OnReload?.Invoke(false);
            OnChargeChanged?.Invoke(_currentCharge);
        }

        private void ReloadHandler()
        {
            if(_currentCharge == _data.MaxCharge)
                return;
            StartCoroutine(ReloadRoutine());
        }
    }
}