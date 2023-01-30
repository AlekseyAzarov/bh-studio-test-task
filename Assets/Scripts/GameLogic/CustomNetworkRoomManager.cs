using Extensions;
using Mirror;
using UnityEngine;

namespace GameLogic
{

    public class CustomNetworkRoomManager : NetworkRoomManager
    {
        public override Transform GetStartPosition()
        {
            startPositions.RemoveAll(t => t == null);

            if (startPositions.Count == 0)
                return null;

            if (playerSpawnMethod == PlayerSpawnMethod.Random)
            {
                return startPositions.GetRandomElement(true);
            }
            else
            {
                Transform startPosition = startPositions[startPositionIndex];
                startPositionIndex = (startPositionIndex + 1) % startPositions.Count;
                return startPosition;
            }
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            Transform startPos = GetStartPosition();
            var gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            var roomPlayerController = roomPlayer.GetComponent<RoomPlayerController>();
            var gamePlayerController = gamePlayer.GetComponent<PlayerController>();

            gamePlayerController.SetPlayerName(roomPlayerController.PlayerName);

            return gamePlayer;
        }
    }
}
