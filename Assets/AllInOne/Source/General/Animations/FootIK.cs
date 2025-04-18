using UnityEngine;

namespace AllInOne
{
    public class FootIK : MonoBehaviour
    {
        [SerializeField] private LocomotionController _locomotionController;
        [SerializeField] private Transform _characterController;
        [SerializeField] private Transform _animatedCharacter;

        private void FixedUpdate()
        {
            _animatedCharacter.GetPositionAndRotation(out Vector3 currentPosition, out Quaternion currentRotation);
            _characterController.GetPositionAndRotation(out Vector3 controllerPosition, out Quaternion controllerRotation);
            
            currentPosition = Vector3.MoveTowards(currentPosition, controllerPosition, _locomotionController.Speed * 1.5f * Time.fixedDeltaTime);
            currentRotation = controllerRotation;
            
            //_animatedCharacter.SetPositionAndRotation(currentPosition, currentRotation);
        }
    }
}