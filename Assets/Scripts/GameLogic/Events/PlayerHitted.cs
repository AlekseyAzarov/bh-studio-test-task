using Observer;

namespace GameLogic
{
    public class PlayerHitted : IObservable
    {
        public PlayerController Hitted;
        public PlayerController Hitter;
    }
}
