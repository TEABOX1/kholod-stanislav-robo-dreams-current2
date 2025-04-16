using UnityEngine;

namespace AllInOne
{
    public class RailGunCharge : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _chargeValue;
        [SerializeField] private Vector2 _referenceSize;
        //[SerializeField] private float _damageDecaySpeed;
        [SerializeField] private HitScanGunCooldown _hitScanGun;
        [SerializeField] private ReloadHUD _hud;


        private InputController _inputController;
        private float _displayedHold;
        private bool _isCharging;

        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();

            _canvasGroup.gameObject.SetActive(false);

            _inputController.OnPrimaryInput += StartFill;
        }

        private void Update()
        {
            if (_isCharging)
            {
                _displayedHold += Time.deltaTime;
                _displayedHold = Mathf.Min(_displayedHold, _hitScanGun.HoldTime);

                float fillRatio = _displayedHold / _hitScanGun.HoldTime;
                _chargeValue.sizeDelta = new Vector2(_referenceSize.x * fillRatio, _referenceSize.y);
            }
        }
        
        private void StartFill(bool isHold)
        {
            if (_hud.enabled)
                return;
            if (isHold)
            {
                _canvasGroup.gameObject.SetActive(true);
                _displayedHold = 0f;
                _isCharging = true;
            }
            else
            {
                _isCharging = false;
                _canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}