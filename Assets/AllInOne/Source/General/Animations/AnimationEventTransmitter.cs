using System;
using UnityEngine;

namespace AllInOne
{
    public class AnimationEventTransmitter : MonoBehaviour
    {
        public event Action onRightFoot;
        public event Action onLeftFoot;
        public event Action onLand;
        public event Action onHit;
        
        public void FootR()
        {
            onRightFoot?.Invoke();
        }

        public void FootL()
        {
            onLeftFoot?.Invoke();
        }

        public void Land()
        {
            onLand?.Invoke();
        }

        public void Hit()
        {
            onHit?.Invoke();
        }
    }
}