using UnityEngine;
using UnityEngine.UI;

namespace AllInOne
{
    public class ReloadHUD : MonoBehaviour
    {
        [SerializeField] private HitScanGunCooldown _gun;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;

        private void Start()
        {
            _gun.OnReload += ReloadHandler;
            enabled = false;
        }

        private void ReloadHandler(bool active)
        {
            _canvasGroup.alpha = active ? 1f : 0f;
            enabled = active;
        }

        private void Update()
        {
            _image.fillAmount = _gun.Reload.Progress;
        }
    }
}