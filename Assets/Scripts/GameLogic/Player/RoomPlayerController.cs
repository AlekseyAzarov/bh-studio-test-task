using Mirror;
using Observer;

namespace GameLogic
{
    public class RoomPlayerController : NetworkBehaviour, IListener<PlayerSetName>
    {
        private string _playerName;

        public string PlayerName => _playerName ??= $"Player_{netId}";

        public override void OnStartClient()
        {
            base.OnStartClient();

            ObserverService.Instance.Subscribe(this);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            ObserverService.Instance.Unsubscribe(this);
        }

        public void Notify(PlayerSetName payload)
        {
            SetPlayerName(payload.Name);
        }

        public void SetPlayerName(string playerName)
        {
            if (!isLocalPlayer) return;
            SetPlayerNameCmd(playerName);
        }

        [Command]
        private void SetPlayerNameCmd(string playerName)
        {
            _playerName = playerName;
            SetPlayerNameRpc(playerName);
        }

        [ClientRpc]
        private void SetPlayerNameRpc(string playerName)
        {
            _playerName = playerName;
        }
    }
}
