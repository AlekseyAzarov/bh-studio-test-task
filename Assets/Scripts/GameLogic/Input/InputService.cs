using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class InputService : AbstractInputService
    {
        private Dictionary<AxisType, (string Horizontal, string Vertical)> _axisTypeAxisNameMap;

        private Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>> _keyActionPairs;
        private Dictionary<AxisType, List<Action<float, float>>> _axisActionPairs;

        private Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>> _keyActionPairsToAdd;
        private Dictionary<AxisType, List<Action<float, float>>> _axisActionPairsToAdd;

        private Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>> _keyActionPairsToRemove;
        private Dictionary<AxisType, List<Action<float, float>>> _axisActionPairsToRemove;

        private void Start()
        {
            _axisTypeAxisNameMap = new Dictionary<AxisType, (string, string)>
            {
                {AxisType.Look, ("Mouse X", "Mouse Y")},
                {AxisType.Movement, ("Horizontal", "Vertical")}
            };

            _keyActionPairs = new Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>>();
            _axisActionPairs = new Dictionary<AxisType, List<Action<float, float>>>();

            _keyActionPairsToAdd = new Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>>();
            _axisActionPairsToAdd = new Dictionary<AxisType, List<Action<float, float>>>();

            _keyActionPairsToRemove = new Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>>();
            _axisActionPairsToRemove = new Dictionary<AxisType, List<Action<float, float>>>();
        }

        private void Update()
        {
            AddInputActions();
            HandleInputActions();
            RemoveInputActions();
        }

        public override void SubscribeToKey(KeyCode targetKey, Action callback, KeyInputType keyInputType = KeyInputType.Down)
        {
            AddKeyActionPairToDictionary(_keyActionPairsToAdd, targetKey, keyInputType, callback);
        }

        public override void UnsubscribeToKey(KeyCode targetKey, Action callback, KeyInputType keyInputType = KeyInputType.Down)
        {
            AddKeyActionPairToDictionary(_keyActionPairsToRemove, targetKey, keyInputType, callback);
        }

        public override void SubscribeToHorizontalAndVerticalAxis(AxisType axisType, Action<float, float> callback)
        {
            if (!_axisActionPairsToAdd.ContainsKey(axisType))
            {
                _axisActionPairsToAdd.Add(axisType, new List<Action<float, float>> { callback });
                return;
            }

            _axisActionPairsToAdd[axisType].Add(callback);
        }

        public override void UnsubscribeToHorizontalAndVerticalAxis(AxisType axisType, Action<float, float> callback)
        {
            if (!_axisActionPairsToRemove.ContainsKey(axisType))
            {
                _axisActionPairsToRemove.Add(axisType, new List<Action<float, float>> { callback });
                return;
            }

            _axisActionPairsToRemove[axisType].Add(callback);
        }

        private void AddKeyActionPairToDictionary(Dictionary<(KeyCode keyCode, KeyInputType inputType), List<Action>> targetDictionary,
            KeyCode targetKey, KeyInputType targetInputType, Action callback)
        {
            if (!targetDictionary.ContainsKey((targetKey, targetInputType)))
            {
                targetDictionary.Add((targetKey, targetInputType), new List<Action> { callback });
                return;
            }

            targetDictionary[(targetKey, targetInputType)].Add(callback);
        }

        private void AddInputActions()
        {
            foreach (var keyActionsToAdd in _keyActionPairsToAdd)
            {
                if (!_keyActionPairs.ContainsKey(keyActionsToAdd.Key))
                {
                    _keyActionPairs.Add(keyActionsToAdd.Key, keyActionsToAdd.Value);
                    continue;
                }

                _keyActionPairs[keyActionsToAdd.Key].AddRange(keyActionsToAdd.Value);
            }

            foreach (var axisActionsToAdd in _axisActionPairsToAdd)
            {
                if (!_axisActionPairs.ContainsKey(axisActionsToAdd.Key))
                {
                    _axisActionPairs.Add(axisActionsToAdd.Key, axisActionsToAdd.Value);
                    continue;
                }

                _axisActionPairs[axisActionsToAdd.Key].AddRange(axisActionsToAdd.Value);
            }

            _keyActionPairsToAdd.Clear();
            _axisActionPairsToAdd.Clear();
        }

        private void HandleInputActions()
        {
            foreach (var inputActionPair in _keyActionPairs)
            {
                switch (inputActionPair.Key.inputType)
                {
                    case KeyInputType.Held:
                        if (Input.GetKey(inputActionPair.Key.keyCode)) foreach (var action in inputActionPair.Value) action?.Invoke();
                        break;
                    case KeyInputType.Down:
                        if (Input.GetKeyDown(inputActionPair.Key.keyCode)) foreach (var action in inputActionPair.Value) action?.Invoke();
                        break;
                    case KeyInputType.Up:
                        if (Input.GetKeyUp(inputActionPair.Key.keyCode)) foreach (var action in inputActionPair.Value) action?.Invoke();
                        break;
                    default:
                        Debug.LogError($"Invalid KeyInputType {inputActionPair.Key}");
                        break;
                }
            }

            foreach (var keyValuePair in _axisActionPairs)
            {
                if (!_axisTypeAxisNameMap.TryGetValue(keyValuePair.Key, out var axisNames))
                {
                    Debug.LogError($"_axisTypeAxisNameMap doesn't contains axis names for {keyValuePair.Key} axis type");
                    continue;
                }

                var horizontal = Input.GetAxis(axisNames.Horizontal);
                var vertical = Input.GetAxis(axisNames.Vertical);

                foreach (var action in keyValuePair.Value)
                {
                    action?.Invoke(horizontal, vertical);
                }
            }
        }

        private void RemoveInputActions()
        {
            foreach (var keyActionsToRemove in _keyActionPairsToRemove)
            {
                if (!_keyActionPairs.ContainsKey(keyActionsToRemove.Key))
                {
                    Debug.LogError($"_keyActionPairs doesn't contains key {keyActionsToRemove.Key.keyCode} of input type {keyActionsToRemove.Key.inputType}");
                    continue;
                }

                _keyActionPairs.Remove(keyActionsToRemove.Key);
            }

            foreach (var axisActionsToRemove in _axisActionPairsToRemove)
            {
                if (!_axisActionPairs.ContainsKey(axisActionsToRemove.Key))
                {
                    Debug.LogError($"_axisActionPairs doesn't contains axis {axisActionsToRemove.Key}");
                    continue;
                }

                _axisActionPairs.Remove(axisActionsToRemove.Key);
            }

            _keyActionPairsToRemove.Clear();
            _axisActionPairsToRemove.Clear();
        }
    }
}
