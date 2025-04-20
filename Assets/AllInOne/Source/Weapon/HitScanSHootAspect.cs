using UnityEngine;

namespace AllInOne
{
    public class HitscanShotAspect : MonoBehaviour
    {
        public MaterialPropertyBlock outerPropertyBlock;

        [HideInInspector] public float distance;

        [SerializeField] private Transform _transform;

        public Transform Transform => _transform;
    }
}