using System;
using System.Collections.Generic;
using Ostium11.Extensions;
using UnityEngine;

namespace Ostium11.TimerTasks
{
    internal class TaskUpdater : MonoBehaviour
    {
        static float _lastManualTickTime = 0f;

        static TaskUpdater _instance;
        static TaskUpdater Instance =>
#pragma warning disable UNT0023
            // bypass unity null check to avoid creating new instance after it was destroyed
            _instance ??= new GameObject($"[{nameof(TaskUpdater)}]").AddComponent<TaskUpdater>();
#pragma warning restore UNT0023

        internal static void Add(UpdateType updateType, Action<float> action)
        {
            Instance._actions[updateType].Add(action);
            Instance._actionUpdateType[action] = updateType;
        }

        internal static void Remove(Action<float> action)
        {
            var updateType = Instance._actionUpdateType[action];
            Instance._actionUpdateType.Remove(action);
            Instance._actions[updateType].Remove(action);
        }

        internal static float GetTime(UpdateType updateType) => updateType switch
        {
            UpdateType.Normal => Time.time,
            UpdateType.Late => Time.time,
            UpdateType.Fixed => Time.fixedTime,
            UpdateType.Manual => _lastManualTickTime,
            _ => throw new ArgumentException($"Unexpected {updateType} update type!")
        };

        internal static void ManualUpdate(float time)
        {
            Instance._actions[UpdateType.Manual].ForEachReversed(a => a(time));
            _lastManualTickTime = time;
        }

        readonly Dictionary<Action<float>, UpdateType> _actionUpdateType = new();
        readonly Dictionary<UpdateType, List<Action<float>>> _actions = new()
        {
            { UpdateType.Normal, new() },
            { UpdateType.Late, new() },
            { UpdateType.Fixed, new() },
            { UpdateType.Manual, new() },
        };

        void Update() => _actions[UpdateType.Normal].ForEachReversed(a => a(Time.time));
        void LateUpdate() => _actions[UpdateType.Late].ForEachReversed(a => a(Time.time));
        void FixedUpdate() => _actions[UpdateType.Fixed].ForEachReversed(a => a(Time.fixedTime));
    }
}