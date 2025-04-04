using System;
using UnityEngine;

namespace AllInOne
{
    [DefaultExecutionOrder(-10)]
    public class DynamicHealthService : DynamicHealthSystem, IHealthService
    {
        public Type Type { get; } = typeof(IHealthService);
        
        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Instance.AddService(this);
        }

        protected virtual void OnDestroy()
        {
            ServiceLocator.Instance.RemoveService(this);
        }
    }
}