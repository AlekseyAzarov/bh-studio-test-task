using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public abstract class AbstractPlayerStateMachine
    {
        protected List<AbstractPlayerState> _playerStates;
        protected AbstractPlayerState _currentState;

        public abstract void Init(PlayerController playerController, AbstractMoveHandler moveHandler, AbstractPlayerStateMachine stateMachine, PlayerAnimationController playerAnimations);

        public virtual void InState()
        {
            _currentState.InState();
        }

        public virtual void Move(Vector3 direction, float yRotation)
        {
            _currentState.Move(direction, yRotation);
        }

        public virtual void Dash()
        {
            _currentState.Dash();
        }

        public virtual void OnCollisionWithPlayer(PlayerController otherPlayer)
        {
            _currentState.OnCollisionWithPlayer(otherPlayer);
        }

        public virtual void ChangeState<T>() where T : AbstractPlayerState
        {
            _currentState.StopState();
            _currentState = _playerStates.Find(x => x is T);
            _currentState.StartState();
        }
    }
}
