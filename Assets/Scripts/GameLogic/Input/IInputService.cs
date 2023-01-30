using System;
using UnityEngine;

namespace GameLogic
{
    public interface IInputService
    {
        void SubscribeToKey(KeyCode targetKey, Action callback, KeyInputType keyInputType = KeyInputType.Down);
        void UnsubscribeToKey(KeyCode targetKey, Action callback, KeyInputType keyInputType = KeyInputType.Down);
        void SubscribeToHorizontalAndVerticalAxis(AxisType axisType, Action<float, float> callback);
        void UnsubscribeToHorizontalAndVerticalAxis(AxisType axisType, Action<float, float> callback);
    }
}
