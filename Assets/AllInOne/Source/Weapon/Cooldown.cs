using System;
using System.Collections;
using UnityEngine;

namespace AllInOne
{
    public class Cooldown
    {
        public event Action<bool> OnCooldown;

        private float _cooldown;
        private float _reciprocal;

        public bool IsOnCooldown { get; private set; }
        public float Progress { get; private set; }

        public Cooldown(float cooldown)
        {
            _cooldown = cooldown;
            _reciprocal = 1f / cooldown;
        }

        public IEnumerator Begin()
        {
            IsOnCooldown = true;

            float time = 0f;

            while (time < _cooldown)
            {
                Progress = time * _reciprocal;
                yield return null;
                time += Time.deltaTime;
            }

            IsOnCooldown = false;
        }
    }
}