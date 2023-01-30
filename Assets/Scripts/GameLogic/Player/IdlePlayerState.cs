using UnityEngine;

namespace GameLogic
{
    public class IdlePlayerState : AbstractPlayerState
    {
        public IdlePlayerState(PlayerController playerController, AbstractMoveHandler moveHandler, AbstractPlayerStateMachine stateMachine, PlayerAnimationController playerAnimations)
            : base(playerController, moveHandler, stateMachine, playerAnimations)
        {
        }

        public override void InState()
        {

        }

        public override void Move(Vector3 direction, float yRotation)
        {
            base.Move(direction, yRotation);

            if (_moveHandler.IsMoving()) _stateMachine.ChangeState<MovePlayerState>();
        }

        public override void StartState()
        {
            _playerAnimations.SetState(PlayerAnimationState.Idle);
        }

        public override void StopState()
        {

        }
    }
}
