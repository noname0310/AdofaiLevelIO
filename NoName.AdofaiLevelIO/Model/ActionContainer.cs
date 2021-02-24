using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model.Actions;
using Action = NoName.AdofaiLevelIO.Model.Actions.Action;

namespace NoName.AdofaiLevelIO.Model
{
    public class ActionContainer : JObjectMaterializer, IEnumerable<Action>
    {
        public int Count => LevelReader.GetFloorActions(JObject, _floorIndex).Count;
        public Action this[int index] 
        {
            get
            {
                switch (Action.ParseEventType(LevelReader.GetFloorActions(JObject, _floorIndex)[index]["eventType"]?.ToString()))
                {
                    case EventType.Twirl:
                        return new Twirl(JObject, _floorIndex, index, _floorCacheContainer);
                    case EventType.SetSpeed:
                        return new SetSpeed(JObject, _floorIndex, index, _floorCacheContainer);
                    case EventType.NotAvailable:
                        goto default;
                    default:
                        return new Action(JObject, _floorIndex, index, _floorCacheContainer);
                }
            }
        }

        private readonly int _floorIndex;
        private readonly FloorCacheContainer _floorCacheContainer;

        internal ActionContainer(JObject jObject, int floorIndex, FloorCacheContainer floorCacheContainer) : base(jObject)
        {
            _floorIndex = floorIndex;
            _floorCacheContainer = floorCacheContainer;
        }

        public void Add(Data.Action action)
        {
            action.Floor = _floorIndex;

            switch (action.EventType)
            {
                case EventType.Twirl:
                    if (Find(EventType.Twirl) != null)
                        break;
                    LevelReader.AddAction(JObject, JToken.FromObject(action));
                    break;
                case EventType.SetSpeed:
                    foreach (var item in _floorCacheContainer)
                        item.Value.Bpm = null;
                    var setSpeedData = (Data.SetSpeed)action;
                    if (Find(EventType.SetSpeed) is SetSpeed setSpeed)
                        setSpeed.SetValue(setSpeedData.SpeedType,
                            (setSpeedData.SpeedType == SpeedType.Bpm)
                                ? setSpeedData.BeatsPerMinute
                                : setSpeedData.BpmMultiplier);
                    else
                        LevelReader.AddAction(JObject, JToken.FromObject(setSpeedData));
                    break;
                case EventType.NotAvailable:
                    goto default;
                default:
                    break;
            }
        }

        public void Remove(EventType eventType)
        {
            var actions = LevelReader.GetActions(JObject);
            for (var i = 0; i < actions.Count; i++)
            {
                if (actions[i]["floor"]?.ToObject<int>() == _floorIndex)
                    LevelReader.RemoveAction(JObject, i);
            }

            if (eventType != EventType.SetSpeed) return;
            foreach (var item in _floorCacheContainer)
                item.Value.Bpm = null;
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
