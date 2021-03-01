using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model.Actions;
using Action = NoName.AdofaiLevelIO.Model.Actions.Action;

namespace NoName.AdofaiLevelIO.Model
{
    public class ActionContainer : IEnumerable<Action>
    {
        internal delegate void ActionChanged(CacheValue cacheValue);
        internal event ActionChanged OnActionChanged;

        public int Count => _actions.Count;
        public Action this[int index] => _actions[index];

        private readonly List<Action> _actions;

        internal ActionContainer(IEnumerable<JToken> actions)
        {
            _actions = actions.Select(actionJToken =>
            {
                Action action;
                switch (Action.ParseEventType(actionJToken["eventType"]?.ToString()))
                {
                    case EventType.Twirl:
                        action = new Twirl(actionJToken);
                        break;
                    case EventType.SetSpeed:
                        action = new SetSpeed(actionJToken);
                        break;
                    case EventType.NotAvailable:
                        goto default;
                    default:
                        action = new Action(actionJToken);
                        break;
                }
                action.ActionChanged += Action_OnActionChanged;
                return action;
            }).ToList();
        }

        private void Action_OnActionChanged(CacheValue cacheValue) => OnActionChanged?.Invoke(cacheValue);

        public void Add(Data.Action action)
        {
            Action instance = null;

            switch (action.EventType)
            {
                case EventType.Twirl:
                    if (Find(EventType.Twirl) != null)
                        break;
                    instance = new Twirl(JToken.FromObject(action));
                    OnActionChanged?.Invoke(CacheValue.IsClockWise);
                    break;
                case EventType.SetSpeed:
                    var setSpeedData = (Data.SetSpeed)action;
                    if (Find(EventType.SetSpeed) is SetSpeed setSpeed)
                        setSpeed.SetValue(setSpeedData.SpeedType,
                            (setSpeedData.SpeedType == SpeedType.Bpm)
                                ? setSpeedData.BeatsPerMinute
                                : setSpeedData.BpmMultiplier);
                    else
                        instance = new SetSpeed(JToken.FromObject(setSpeedData));
                    OnActionChanged?.Invoke(CacheValue.Bpm);
                    break;
                case EventType.NotAvailable:
                    goto default;
                default:
                    break;
            }

            if (instance == null) return;
            instance.ActionChanged += Action_OnActionChanged;
            _actions.Add(instance);
        }

        public void Remove(EventType eventType)
        {
            for (var i = 0; i < _actions.Count; i++)
            {
                if (_actions[i].EventType != eventType) continue;
                _actions.RemoveAt(i);
                switch (eventType)
                {
                    case EventType.SetSpeed:
                        OnActionChanged?.Invoke(CacheValue.Bpm);
                        break;
                    case EventType.Twirl:
                        OnActionChanged?.Invoke(CacheValue.IsClockWise);
                        break;
                    case EventType.NotAvailable:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
                }
                break;
            }

            
        }

        public Action Find(EventType eventType) => this.FirstOrDefault(action => action.EventType == eventType);

        public IEnumerator<Action> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
