using Mirror;

namespace GameLogic
{
    public abstract class AbstractPlayerConfigurator : NetworkBehaviour
    {
        public abstract void ConfigurePlayer(PlayerController playerController);
    }
}
