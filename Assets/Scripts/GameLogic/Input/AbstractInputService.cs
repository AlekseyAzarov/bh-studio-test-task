using System;
using UnityEngine;

namespace GameLogic
{
    public abstract class AbstractInputService : MonoBehaviour, IInputService
    {
        public abstract void SubscribeToHorizontalAndVerticalAxis(AxisType axisType, Action<float, float> callback);

        public abstract void SubscribeToKey(KeyCode targetKey, Action callback, KeyInputType keyInputType = KeyInputType.Down);

        public abstract void UnsubscribeToHorizontalAndVerticalAxis(AxisType axisType, Action<float, float> callback);

        public abstract void UnsubscribeToKey(KeyCode targetKey, Action callback, KeyInputType keyInputType = KeyInputType.Down);
    }
}
