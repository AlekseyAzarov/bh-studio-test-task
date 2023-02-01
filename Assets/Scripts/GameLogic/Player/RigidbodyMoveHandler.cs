using Mirror;
using Mirror.Experimental;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMoveHandler : AbstractMoveHandler
    {
        [SyncVar] private Vector3 _direction;
        [SyncVar] private float _yRotation;
        [SyncVar] private float _speed;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;

            _rigidbody.MoveRotation(Quaternion.Euler(0f, _yRotation, 0f));
            var direction = _rigidbody.rotation * _direction;
            _rigidbody.velocity = direction.normalized * _speed;
        }

        [Server]
        public override void Stop()
        {
            _direction = Vector3.zero;
        }

        [Server]
        public override void Move(Vector3 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        [Server]
        public override void RotateY(float yRotation)
        {
            _yRotation = yRotation;
        }

        public override bool IsMoving()
        {
            return _direction.magnitude > 0f && _speed > 0f;
        }

        private void OnMoveParametersChanged(object oldValue, object newValue)
        {
            if (!isLocalPlayer) return;

            _rigidbody.MoveRotation(Quaternion.Euler(0f, _yRotation, 0f));
            var direction = _rigidbody.rotation * _direction;
            _rigidbody.velocity = direction * _speed;
        }
    }
}
