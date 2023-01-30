using Mirror;
using System.Collections;
using UnityEngine;

namespace GameLogic
{
    public class DashPlayerState : AbstractPlayerState
    {
        private readonly Vector3 _direction = new Vector3(0f, 0f, 1f);

        private bool _ignoreDistance;
        private float _dashDistance;
        private float _dashSpeed;
        private Vector3 _startPosition;
        private Coroutine _dashRoutine;

        public DashPlayerState(PlayerController playerController, AbstractMoveHandler moveHandler, AbstractPlayerStateMachine stateMachine, PlayerAnimationController playerAnimations)
            : base(playerController, moveHandler, stateMachine, playerAnimations)
        {
        }

        public override void Dash()
        {

        }

        public override void InState()
        {
        }

        public override void Move(Vector3 direction, float yRotation)
        {

        }

        public override void OnCollisionWithPlayer(PlayerController otherPlayer)
        {
            base.OnCollisionWithPlayer(otherPlayer);

            _playerController.StopCoroutine(_dashRoutine);
            otherPlayer.OnHittedByDash(_playerController);
        }

        public override void StartState()
        {
            _playerAnimations.SetState(PlayerAnimationState.Dash);
            _startPosition = _playerController.transform.position;
            _dashDistance = _playerController.DashDistance;
            _dashSpeed = _playerController.DashSpeed;
            _ignoreDistance = false;

            _dashRoutine = _playerController.StartCoroutine(DashRoutine());
        }

        public override void StopState()
        {
        }

        private IEnumerator DashRoutine()
        {
            _playerController.PlayerCollidedWithObstacle += OnCollidedWithObstacle;
            var time = NetworkTime.time;
            var position = _playerController.transform.position;

            while (Vector3.Distance(position, _startPosition) < _dashDistance && _ignoreDistance == false)
            {
                _moveHandler.Move(_direction, _dashSpeed);
                position = _playerController.transform.position;
                yield return new WaitForEndOfFrame();
            }

            var dashStunEndTime = NetworkTime.time + _playerController.DashStunTime;
            _moveHandler.Stop();
            _playerController.PlayerCollidedWithObstacle -= OnCollidedWithObstacle;
            _playerAnimations.SetState(PlayerAnimationState.Idle);

            yield return new WaitUntil(() => NetworkTime.time > dashStunEndTime);

            _stateMachine.ChangeState<IdlePlayerState>();
        }

        private void OnCollidedWithObstacle()
        {
            _ignoreDistance = true;
        }
    }
}
