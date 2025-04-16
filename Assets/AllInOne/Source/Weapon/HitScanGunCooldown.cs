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
        private bool _canShoot = true;
        private bool _isPrimaryHeld;

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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void PrimaryInputHandler(bool isHold)
        {
            _isPrimaryHeld = isHold;

            if (_reload.IsOnCooldown)
            {
                if (!isHold)
                    _canShoot = true;
                return;
            }

            if (!_canShoot)
            {
                if (!isHold)
                    _canShoot = true;
                return;
            }

            if (_cooldown.IsOnCooldown || isHold)
                return;
            base.PrimaryInputHandler(isHold);
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
            _canShoot = false;
            OnReload?.Invoke(true);
            yield return _reload.Begin();
            _currentCharge = _data.MaxCharge;
            OnReload?.Invoke(false);
            OnChargeChanged?.Invoke(_currentCharge);
        }

        private void ReloadHandler()
        {
            if (_isPrimaryHeld || _currentCharge == _data.MaxCharge)
                return;
            StartCoroutine(ReloadRoutine());
        }
    }
}