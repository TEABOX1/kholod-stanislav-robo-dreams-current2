using TMPro;
using UnityEngine;

namespace AllInOne
{
    public class ChargeHUD : MonoBehaviour
    {
        [SerializeField] private HitScanGunCooldown _gun;
        [SerializeField] private TextMeshProUGUI _chargeValue;
        [SerializeField] private TextMeshProUGUI _maxCharge;

        private void Start()
        {
            _chargeValue.text = _gun.CurrentCharge.ToString();
            _maxCharge.text = _gun.MaxCharge.ToString();
            _gun.OnChargeChanged += ChargeHandler;
        }

        private void ChargeHandler(int charge)
        {
            _chargeValue.text = charge.ToString();
        }
    }
}