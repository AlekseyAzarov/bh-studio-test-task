using Mirror;
using Observer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
    public class GameStatisticController : NetworkBehaviour, IListener<PlayerSetName>, IListener<PlayerHitted>, IListener<RoundStarted>
    {
        [SerializeField] private GameSettingsData _gameSettingsData;

        private Dictionary<string, int> _playerScorePairs = new Dictionary<string, int>();

        public override void OnStartServer()
        {
            ObserverService.Instance.Subscribe<PlayerSetName>(this);
            ObserverService.Instance.Subscribe<PlayerHitted>(this);
            ObserverService.Instance.Subscribe<RoundStarted>(this);
        }

        public override void OnStopServer()
        {
            ObserverService.Instance.Unsubscribe<PlayerSetName>(this);
            ObserverService.Instance.Unsubscribe<PlayerHitted>(this);
            ObserverService.Instance.Unsubscribe<RoundStarted>(this);
        }

        public void Notify(PlayerSetName payload)
        {
            _playerScorePairs.Add(payload.Name, 0);
            UpdateStatisticRpc(_playerScorePairs.Keys.ToList(), _playerScorePairs.Values.ToList());
        }

        public void Notify(PlayerHitted payload)
        {
            var hitterName = payload.Hitter.PlayerName;

            if (!_playerScorePairs.ContainsKey(hitterName))
            {
                Debug.LogError($"Player with name: {hitterName} is not registered in statistic controller");
                return;
            }

            _playerScorePairs[hitterName]++;

            UpdateStatisticRpc(_playerScorePairs.Keys.ToList(), _playerScorePairs.Values.ToList());

            if (_playerScorePairs[hitterName] == _gameSettingsData.HitForWin)
            {
                ObserverService.Instance.Unsubscribe<PlayerHitted>(this);
                ObserverService.Instance.RaiseEvent(new RoundRestarting { });
                OnPlayerWonRpc(hitterName);
            }
        }

        public void Notify(RoundStarted payload)
        {
            var keys = _playerScorePairs.Keys.ToList();
            foreach (var playerScoreKey in keys)
            {
                _playerScorePairs[playerScoreKey] = 0;
            }

            ObserverService.Instance.Subscribe<PlayerHitted>(this);
            UpdateStatisticRpc(_playerScorePairs.Keys.ToList(), _playerScorePairs.Values.ToList());
        }

        [ClientRpc]
        private void UpdateStatisticRpc(List<string> names, List<int> points)
        {
            var playerScorePairs = names
                .Zip(points, (name, point) => new { name, point })
                .ToDictionary(x => x.name, x => x.point);

            ObserverService.Instance.RaiseEvent(new StatisticUpdated { PlayerScorePairs = playerScorePairs });
        }

        [ClientRpc]
        private void OnPlayerWonRpc(string playerName)
        {
            ObserverService.Instance.RaiseEvent(new PlayerWon { WinnerName = playerName });
        }
    }
}
