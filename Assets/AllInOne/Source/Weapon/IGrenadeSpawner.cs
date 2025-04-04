using System;
using UnityEngine;

namespace AllInOne
{
    public abstract class GrenadeSpawnerBase : MonoBehaviour
    {
        public event Action<Grenade> OnGrenadeSpawned;

        protected void InvokeOnGrenadeSpawned(Grenade grenade) => OnGrenadeSpawned?.Invoke(grenade);
    }
}