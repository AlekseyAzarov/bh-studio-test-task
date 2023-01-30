using Observer;
using System.Collections.Generic;

namespace GameLogic
{
    public class StatisticUpdated : IObservable
    {
        public Dictionary<string, int> PlayerScorePairs;
    }
}
