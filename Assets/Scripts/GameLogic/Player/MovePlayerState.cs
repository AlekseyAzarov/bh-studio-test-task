namespace GameLogic
{
    public class MovePlayerState : AbstractPlayerState
    {
        public MovePlayerState(PlayerController playerController, AbstractMoveHandler moveHandler, AbstractPlayerStateMachine stateMachine, PlayerAnimationController playerAnimations)
            : base(playerController, moveHandler, stateMachine, playerAnimations)
        {
        }

        public override void InState()
        {
            if (!_moveHandler.IsMoving()) _stateMachine.ChangeState<IdlePlayerState>();
        }

        public override void StartState()
        {

        }

        public override void StopState()
        {

        }
    }
}
