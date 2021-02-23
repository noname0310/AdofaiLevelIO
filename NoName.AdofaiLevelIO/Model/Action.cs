using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model
{
    public class Action : JObjectMaterializer
    {
        public EventType EventType => ParseEventType(LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["eventType"]?.ToString());

        protected readonly int FloorIndex;
        protected readonly int ActionIndex;

        internal Action(JObject jObject, int floorIndex, int actionIndex) : base(jObject)
        {
            if (LevelReader.GetFloorActions(jObject, floorIndex).Count <= actionIndex)
                throw new IndexOutOfRangeException();
            FloorIndex = floorIndex;
            ActionIndex = actionIndex;
        }

        internal static EventType ParseEventType(string eventType)
        {
            switch (eventType)
            {
                case "Twirl":
                    return EventType.Twirl;
                case "SetSpeed":
                    return EventType.SetSpeed;
                default:
                    return EventType.NotAvailable;
            }
        }
    }

    public enum EventType
    {
        Twirl,
        SetSpeed,
        NotAvailable
    }
}
