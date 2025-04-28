using System;
using UnityEngine;

namespace AllInOne
{
    public class ReloadSoundController : MonoBehaviour
    {
        [SerializeField] private HitScanGunCooldown _gun;
        [SerializeField] private AudioSource _source;
        
        private void Start()
        {
            _gun.OnReload += ReloadHandler;
        }

        private void ReloadHandler(bool began)
        {
            if (!began)
                return;
            if (_source.isPlaying)
                _source.Stop();
            _source.Play();
        }
    }
}