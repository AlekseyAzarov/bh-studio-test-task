using Mirror;

namespace GameLogic
{
    public abstract class AbstractGameRoundController : NetworkBehaviour
    {
        public abstract void StartRound();
        public abstract void StopRound();
    }
}
