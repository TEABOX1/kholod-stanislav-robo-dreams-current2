using UnityEngine;

namespace AllInOne
{
    public class GrenadeSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _fuzeSource;
        [SerializeField] private AudioSource _explosionSource;
        [SerializeField] private AudioClip _fuseClip;
        [SerializeField] private AudioClip[] _explodeClips;
        [SerializeField] private GrenadeAction _grenadeAction;

        private void Start()
        {
            _grenadeAction.OnGrenadeSpawned += GrenadeSpawnHandler;
        }

        private void GrenadeSpawnHandler(Grenade grenade)
        {
            _fuzeSource.transform.position = grenade.Position;
            _fuzeSource.PlayOneShot(_fuseClip);
            grenade.OnExplode += ExplosionHandler;
        }

        private void ExplosionHandler(Grenade grenade)
        {
            _explosionSource.transform.position = grenade.Position;
            _explosionSource.PlayOneShot(_explodeClips[Random.Range(0, _explodeClips.Length)]);
        }
    }
}