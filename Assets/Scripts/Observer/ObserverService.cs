using System;
using System.Collections.Generic;
using UnityEngine;

namespace Observer
{
    public class ObserverService
    {
        private static ObserverService _singleton;

        private Dictionary<Type, List<object>> _eventListenersPairs;

        public static ObserverService Instance => _singleton ??= new ObserverService();

        private ObserverService()
        {
            _eventListenersPairs = new Dictionary<Type, List<object>>();
        }

        public void RaiseEvent<T>(T observable) where T : class, IObservable
        {
            if (!_eventListenersPairs.TryGetValue(typeof(T), out var listeners))
            {
                Debug.Log($"Event of type {typeof(T)} has 0 listeners");
                return;
            }

            var listenersCount = listeners.Count;
            for (int i = 0; i < listenersCount; i++)
            {
                if (i >= listeners.Count) break;
                var listener = listeners[i];
                var castedListener = (IListener<T>)listener;
                castedListener.Notify(observable);
            }
        }

        public void Subscribe<T>(IListener<T> listener) where T : class, IObservable
        {
            if (!_eventListenersPairs.TryGetValue(typeof(T), out var listeners))
            {
                _eventListenersPairs.Add(typeof(T), new List<object> { listener });
                return;
            }

            listeners.Add(listener);
        }

        public void Unsubscribe<T>(IListener<T> listener) where T : class, IObservable
        {
            if (!_eventListenersPairs.TryGetValue(typeof(T), out var listeners))
            {
                return;
            }

            listeners.Remove(listener);
        }
    }
}
