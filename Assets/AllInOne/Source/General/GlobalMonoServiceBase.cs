using AllInOne;
using System;
using UnityEngine;

namespace AllInOne
{
    [DefaultExecutionOrder(-20)]
    public abstract class GlobalMonoServiceBase : MonoServiceBase
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}