using Mirror;
using Observer;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic
{
    [RequireComponent(typeof(AbstractMoveHandler), typeof(PlayerAnimationController), typeof(ColorChangeHandler))]
    public class PlayerController : NetworkBehaviour
    {
        public event Action PlayerCollidedWithObstacle;

        private float _speed;
        private float _dashDistance;
        private float _dashSpeed;
        private float _dashStunTime;
        private float _changeColorTime;
        private bool _isColorChanged;
        private string _name;
        private Color _colorAfterHit;

        private ColorChangeHandler _colorChangeHandler;
        private AbstractPlayerStateMachine _playerStateMachine;

        public float Speed => _speed;
        public float DashDistance => _dashDistance;
        public float DashSpeed => _dashSpeed;
        public float DashStunTime => _dashStunTime;
        public string PlayerName => _name;

        public void Init(float speed, float dashDistance, float dashSpeed, float dashStunTime, float changeColorTime, Color clorAfterHit)
        {
            _speed = speed;
            _dashDistance = dashDistance;
            _dashSpeed = dashSpeed;
            _changeColorTime = changeColorTime;
            _dashStunTime = dashStunTime;
            _colorAfterHit = clorAfterHit;

            var moveHandler = GetComponent<AbstractMoveHandler>();
            var animations = GetComponent<PlayerAnimationController>();
            _playerStateMachine = new GameModPlayerStateMachine();
            _playerStateMachine.Init(this, moveHandler, _playerStateMachine, animations);
        }

        private void Start()
        {
            _colorChangeHandler = GetComponent<ColorChangeHandler>();
            var playerSpawned = new PlayerSpawned { PlayerController = this };
            ObserverService.Instance.RaiseEvent(playerSpawned);
        }

        private void Update()
        {
            if (!isServer) return;

            _playerStateMachine?.InState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isServer) return;

            if (collision.gameObject.TryGetComponent<PlayerController>(out var otherPlayer))
            {
                _playerStateMachine.OnCollisionWithPlayer(otherPlayer);
                return;
            }

            PlayerCollidedWithObstacle?.Invoke();
        }

        [Server]
        public void SetPlayerName(string name)
        {
            _name = name;
            ObserverService.Instance.RaiseEvent(new PlayerSetName { Name = _name });
        }

        [Command]
        public void Move(Vector3 direction, float yRotation)
        {
            _playerStateMachine.Move(direction, yRotation);
        }

        [Command]
        public void Dash()
        {
            _playerStateMachine.Dash();
        }

        [Server]
        public void OnHittedByDash(PlayerController hitSource)
        {
            if (_isColorChanged == true) return;

            var playerHitted = new PlayerHitted { Hitted = this, Hitter = hitSource };
            ObserverService.Instance.RaiseEvent(playerHitted);
            StartCoroutine(ChangeColorRoutine());
        }

        [ClientRpc]
        private void OnHittedByDashRpc(Color targetColor)
        {
            _colorChangeHandler.ChangeColor(targetColor);
        }

        [ClientRpc]
        private void OnChageColorTimePassedRpc()
        {
            _colorChangeHandler.ReturnColor();
        }

        private IEnumerator ChangeColorRoutine()
        {
            OnHittedByDashRpc(_colorAfterHit);
            _isColorChanged = true;
            yield return new WaitForSeconds(_changeColorTime);
            _isColorChanged = false;
            OnChageColorTimePassedRpc();
        }
    }
}
