using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model.Actions;

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
                        return new Twirl(JObject, _floorIndex, index);
                    case EventType.SetSpeed:
                        return new SetSpeed(JObject, _floorIndex, index);
                    case EventType.NotAvailable:
                        goto default;
                    default:
                        return new Action(JObject, _floorIndex, index);
                }
            }
        }

        private readonly int _floorIndex;

        internal ActionContainer(JObject jObject, int floorIndex) : base(jObject) => _floorIndex = floorIndex;

        public IEnumerator<Action> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
