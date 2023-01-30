using Mirror;
using UnityEngine;

namespace GameLogic
{

    public class PlayerAnimationController : NetworkBehaviour
    {
        private const string STATE_PROPERTY = "State";
        private const string X_DIRECTION_PROPERTY = "XDirection";
        private const string Y_DIRECTION_PROPERTY = "YDirection";

        [SerializeField] private Animator _animator;

        public void SetState(PlayerAnimationState playerAnimationState)
        {
            _animator.SetInteger(STATE_PROPERTY, (int)playerAnimationState);
        }

        public void SetMovementValues(float horizontal, float vertical)
        {
            horizontal = Mathf.Clamp(horizontal, -1f, 1f);
            vertical = Mathf.Clamp(vertical, -1f, 1f);

            _animator.SetFloat(X_DIRECTION_PROPERTY, horizontal);
            _animator.SetFloat(Y_DIRECTION_PROPERTY, vertical);
        }
    }
}
