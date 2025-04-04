using System;
using UnityEngine;

namespace AllInOne
{
    public class CameraService : MonoServiceBase, ICameraService
    {
        [SerializeField] private Camera _camera;
        public override Type Type { get; } = typeof(ICameraService);
        public Camera Camera => _camera;
    }
}