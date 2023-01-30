using UnityEngine;

namespace GameLogic
{
    public abstract class AbstractPlayerState
    {
        protected PlayerController _playerController;
        protected AbstractMoveHandler _moveHandler;
        protected AbstractPlayerStateMachine _stateMachine;
        protected PlayerAnimationController _playerAnimations;

        public AbstractPlayerState(PlayerController playerController,
            AbstractMoveHandler moveHandler,
            AbstractPlayerStateMachine stateMachine,
            PlayerAnimationController playerAnimations)
        {
            _playerController = playerController;
            _moveHandler = moveHandler;
            _stateMachine = stateMachine;
            _playerAnimations = playerAnimations;
        }

        public abstract void InState();
        public abstract void StartState();
        public abstract void StopState();

        public virtual void Move(Vector3 direction, float yRotation)
        {
            _moveHandler.RotateY(yRotation);
            _moveHandler.Move(direction, _playerController.Speed);
            _playerAnimations.SetMovementValues(direction.x, direction.z);
            if (direction.magnitude > 0f) _playerAnimations.SetState(PlayerAnimationState.Run);
        }

        public virtual void Dash()
        {
            _stateMachine.ChangeState<DashPlayerState>();
        }

        public virtual void OnCollisionWithPlayer(PlayerController otherPlayer)
        {
            _stateMachine.ChangeState<IdlePlayerState>();
        }
    }
}
