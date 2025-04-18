using System.Collections;
using UnityEngine;

namespace AllInOne
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _deathName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private HandsIK _handsIK;
        [SerializeField] private float _healthBarDelayTime;
        [SerializeField] private GameObject _logicalPlayer;

        private IHealth _health;

        private void Start()
        {
            _health = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<IHealth>();
            _health.OnDeath += DeathHandler;
        }

        private void DeathHandler()
        {
            StartCoroutine(DelayedDestroy());
        }

        private IEnumerator DelayedDestroy()
        {
            ServiceLocator.Instance.GetService<InputController>().enabled = false;
            //_logicalPlayer.SetActive(false);
            yield return null;

            _animator.CrossFadeInFixedTime(_deathName, _crossFadeTime);
            _handsIK.DisableIK();

            yield return new WaitForSeconds(_healthBarDelayTime);

            ServiceLocator.Instance.GetService<ISaturationService>().SetDeathSaturation();
        }
    }
}