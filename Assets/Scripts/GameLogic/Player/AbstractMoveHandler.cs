using Mirror;
using UnityEngine;

namespace GameLogic
{
    public abstract class AbstractMoveHandler : MonoBehaviour
    {
        public abstract void Stop();
        public abstract void Move(Vector3 direction, float speed);
        public abstract void RotateY(float yRotation);
        public abstract bool IsMoving();
    }
}
