using Observer;

namespace GameLogic
{
    public class PlayerWon : IObservable
    {
        public string WinnerName;
    }
}
