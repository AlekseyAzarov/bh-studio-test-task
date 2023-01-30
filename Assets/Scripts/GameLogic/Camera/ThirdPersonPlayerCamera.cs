using UnityEngine;

namespace GameLogic
{
    public class ThirdPersonPlayerCamera : AbstractPlayerCamera
    {
        [SerializeField] private float _offsetDistance;
        [SerializeField] private Transform _lookAtTarget;

        public override void UpdateTransform()
        {
            base.UpdateTransform();

            _camera.transform.position = _lookAtTarget.position - _camera.transform.forward * _offsetDistance;
            _camera.transform.LookAt(_lookAtTarget.position);
        }
    }
}
