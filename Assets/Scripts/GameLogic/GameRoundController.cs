using Extensions;
using Mirror;
using Observer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
    public class GameRoundController : AbstractGameRoundController, IListener<RoundRestarting>
    {
        [SerializeField] private GameSettingsData _gameSettingsData;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private List<Transform> _usedSpawnPoints;

        public override void OnStartServer()
        {
            ObserverService.Instance.Subscribe(this);
        }

        public override void OnStopServer()
        {
            ObserverService.Instance.Unsubscribe(this);
        }

        [Server]
        public override void StartRound()
        {
            var players = NetworkServer.connections.Values.Select(x => x.identity.gameObject).ToList();
            var spawnPoints = new List<Transform>(_spawnPoints);

            foreach (var player in players)
            {
                var spawnPoint = spawnPoints.GetRandomElement(true);
                var netTransorm = player.GetComponent<NetworkTransformBase>();
                netTransorm.RpcTeleport(spawnPoint.position);
            }

            ObserverService.Instance.RaiseEvent(new RoundStarted());
        }

        [Server]
        public override void StopRound()
        {

        }

        public void Notify(RoundRestarting payload)
        {
            StopRound();
            StartCoroutine(RestartRoutine(_gameSettingsData.TimeBeforeRestart));
        }

        private IEnumerator RestartRoutine(float waitThreshold)
        {
            yield return new WaitForSeconds(waitThreshold);

            StartRound();
        }
    }
}
