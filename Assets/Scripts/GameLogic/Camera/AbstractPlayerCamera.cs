using UnityEngine;

namespace GameLogic
{
    public abstract class AbstractPlayerCamera : MonoBehaviour
    {
        [SerializeField] private float _maxXRotation;
        [SerializeField] private float _minXRotation;

        protected Vector3 _currentRotation;
        protected Camera _camera;
        protected float _additiveXRotation;
        protected float _additiveYRotation;

        public float CameraYRotation => _camera.transform.localRotation.eulerAngles.y;

        public virtual void UpdateTransform()
        {
            _currentRotation = new Vector3(_additiveXRotation, _additiveYRotation);
            _camera.transform.localEulerAngles = _currentRotation;
        }

        public virtual void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public virtual void UpdateRotation(float x, float y)
        {
            _additiveXRotation += x;
            _additiveYRotation += y;

            _additiveXRotation = Mathf.Clamp(_additiveXRotation, _minXRotation, _maxXRotation);
        }
    }
}
