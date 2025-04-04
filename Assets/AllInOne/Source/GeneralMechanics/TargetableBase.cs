using UnityEngine;

namespace AllInOne
{
    public class TargetableBase : MonoBehaviour, ITargetable
    {
        [SerializeField] private Transform _targetPivot;

        public Transform TargetPivot => _targetPivot;
    }
}