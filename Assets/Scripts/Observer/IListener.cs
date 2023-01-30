namespace Observer
{
    public interface IListener<T> where T : IObservable
    {
        void Notify(T payload);
    }
}
