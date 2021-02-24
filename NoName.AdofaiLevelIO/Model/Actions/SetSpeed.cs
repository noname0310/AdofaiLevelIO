using System;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model.Actions
{
    public class SetSpeed : Action
    {
        public SpeedType SpeedType
        {
            get => ParseSpeedType(LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["speedType"]?.ToString());
            private set => LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["speedType"] = SpeedType2String(value);
        }

        public float Value
        {
            get
            {
                switch (SpeedType)
                {
                    case SpeedType.Bpm:
                        return LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["beatsPerMinute"]?
                            .ToObject<float>() ?? throw new Exception("Value not exist");
                    case SpeedType.Multiplier:
                        return LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["bpmMultiplier"]?
                            .ToObject<float>() ?? throw new Exception("Value not exist");
                    case SpeedType.NotAvailable:
                        goto case default;
                    default:
                        throw new Exception("Value not exist");
                }
            }
            private set
            {
                switch (SpeedType)
                {
                    case SpeedType.Bpm:
                        LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["beatsPerMinute"] = value;
                        break;
                    case SpeedType.Multiplier:
                        LevelReader.GetFloorActions(JObject, FloorIndex)[ActionIndex]["bpmMultiplier"] = value;
                        break;
                    case SpeedType.NotAvailable:
                        goto case default;
                    default:
                        throw new Exception("Value not exist");
                }
            }
        }

        internal SetSpeed(JObject jObject, int floorIndex, int actionIndex, FloorCacheContainer floorCacheContainer) 
            : base(jObject, floorIndex, actionIndex, floorCacheContainer) { }

        public void SetValue(SpeedType speedType, float value)
        {
            foreach (var item in FloorCacheContainer)
                item.Value.Bpm = null;
            SpeedType = speedType;
            Value = value;
        }

        internal static SpeedType ParseSpeedType(string speedType)
        {
            switch (speedType)
            {
                case "Bpm":
                    return SpeedType.Bpm;
                case "Multiplier":
                    return SpeedType.Multiplier;
                default:
                    return SpeedType.NotAvailable;
            }
        }

        internal static string SpeedType2String(SpeedType speedType)
        {
            switch (speedType)
            {
                case SpeedType.Bpm:
                    return "Bpm";
                case SpeedType.Multiplier:
                    return "Multiplier";
                case SpeedType.NotAvailable:
                    goto case default;
                default:
                    return "NotAvailable";
            }
        }
    }

    public enum SpeedType
    {
        Bpm,
        Multiplier,
        NotAvailable
    }
}
