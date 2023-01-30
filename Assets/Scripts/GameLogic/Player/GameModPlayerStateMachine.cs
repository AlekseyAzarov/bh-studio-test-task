using System.Collections.Generic;

namespace GameLogic
{
    public class GameModPlayerStateMachine : AbstractPlayerStateMachine
    {
        public override void Init(PlayerController playerController, AbstractMoveHandler moveHandler, AbstractPlayerStateMachine stateMachine, PlayerAnimationController playerAnimations)
        {
            _playerStates = new List<AbstractPlayerState>
            {
                new IdlePlayerState(playerController, moveHandler, stateMachine, playerAnimations),
                new MovePlayerState(playerController, moveHandler, stateMachine, playerAnimations),
                new DashPlayerState(playerController, moveHandler, stateMachine, playerAnimations)
            };

            _currentState = _playerStates[0];
            _currentState.StartState();
        }
    }
}
