using Mirror;
using Observer;
using UnityEngine;

namespace GameLogic
{
    public class PlayerConfigurator : AbstractPlayerConfigurator, IListener<PlayerSpawned>
    {
        [SerializeField] private AbstractInputService _inputService;
        [SerializeField] private PlayerSettingsData _playerSettings;
        [SerializeField] private Camera _playerCamera;

        private void Awake()
        {
            var a = isServer;
            var b = isClient;

            ObserverService.Instance.Subscribe(this);
        }

        private void OnDestroy()
        {
            ObserverService.Instance.Unsubscribe(this);
        }

        public override void ConfigurePlayer(PlayerController playerController)
        {
            ConfigureNonLocalPlayer(playerController);
            ConfigureLocalPlayer(playerController);
        }

        public void Notify(PlayerSpawned payload)
        {
            ConfigurePlayer(payload.PlayerController);
        }

        [ClientCallback]
        private void ConfigureLocalPlayer(PlayerController playerController)
        {
            var isLocalPlayer = playerController.isLocalPlayer;
            var localPlayerController = playerController.gameObject.GetComponent<LocalPlayerController>();
            var playerCamera = playerController.gameObject.GetComponent<AbstractPlayerCamera>();

            if (isLocalPlayer)
            {
                localPlayerController.Init(_inputService, _playerSettings.LookSensivity);
                playerCamera.SetCamera(_playerCamera);
                return;
            }

            Destroy(localPlayerController);
            Destroy(playerCamera);
        }

        [ServerCallback]
        private void ConfigureNonLocalPlayer(PlayerController playerController)
        {
            playerController.Init(_playerSettings.PlayerSpeed,
                _playerSettings.DashDistance,
                _playerSettings.DashSpeed,
                _playerSettings.DashStunTime,
                _playerSettings.ChangeColorTime,
                _playerSettings.ColorAfterHit);
        }
    }
}
