using Mirror;
using Mirror.Experimental;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMoveHandler : AbstractMoveHandler
    {
        private Vector3 _direction;
        private float _speed;
        private float _yRotation;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        public override void Move(Vector3 direction, float speed)
        {
            direction = _rigidbody.rotation * direction;
            _rigidbody.velocity = direction.normalized * speed;
        }

        public override void RotateY(float yRotation)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0f, yRotation, 0f));
        }

        public override bool IsMoving()
        {
            return _rigidbody.velocity.magnitude > 0f;
        }
    }
}
