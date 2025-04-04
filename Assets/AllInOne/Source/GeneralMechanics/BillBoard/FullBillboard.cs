using UnityEngine;

namespace AllInOne
{
    public class FullBillboard : BillboardBase
    {
        private Transform _cameraTransform;
        private Camera _camera;
        private Transform _transform;
        
        public override void SetCamera(Camera camera)
        {
            _transform = transform;
            _camera = camera;
            _cameraTransform = _camera.transform;
        }

        private void LateUpdate()
        {
            Vector3 direction = -1*(_camera.transform.position - _transform.position).normalized;
            _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}