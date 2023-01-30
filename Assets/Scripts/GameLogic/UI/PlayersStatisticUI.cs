using Observer;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameLogic
{
    public class PlayersStatisticUI : MonoBehaviour, IListener<StatisticUpdated>, IListener<PlayerWon>
    {
        [SerializeField] private GameObject _playerPlacesParent;
        [SerializeField] private List<TextMeshProUGUI> _playerPlaceText;
        [SerializeField] private TextMeshProUGUI _winnerNameText;

        private void Awake()
        {
            ObserverService.Instance.Subscribe<StatisticUpdated>(this);
            ObserverService.Instance.Subscribe<PlayerWon>(this);
        }

        private void OnDestroy()
        {
            ObserverService.Instance.Unsubscribe<StatisticUpdated>(this);
            ObserverService.Instance.Unsubscribe<PlayerWon>(this);
        }

        public void Notify(StatisticUpdated payload)
        {
            _playerPlacesParent.SetActive(true);
            _winnerNameText.gameObject.SetActive(false);

            var sortedPlaces = payload.PlayerScorePairs.OrderByDescending(x => x.Value).Take(_playerPlaceText.Count).ToList();

            for (int i = 0; i < _playerPlaceText.Count; i++)
            {
                if (i >= sortedPlaces.Count)
                {
                    _playerPlaceText[i].text = string.Empty;
                    continue;
                }

                var name = sortedPlaces[i].Key;
                var score = sortedPlaces[i].Value;

                _playerPlaceText[i].text = $"{name}: {score}";
            }
        }

        public void Notify(PlayerWon payload)
        {
            _playerPlacesParent.SetActive(false);
            _winnerNameText.gameObject.SetActive(true);
            _winnerNameText.text = $"Winner: {payload.WinnerName}";
        }
    }
}
