using System;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model.Actions
{
    public class SetSpeed : Action
    {
        public SpeedType SpeedType
        {
            get => ParseSpeedType(ActionJToken["speedType"]?.ToString());
            private set => ActionJToken["speedType"] = SpeedType2String(value);
        }

        public float Value
        {
            get
            {
                switch (SpeedType)
                {
                    case SpeedType.Bpm:
                        return ActionJToken["beatsPerMinute"]?
                            .ToObject<float>() ?? throw new Exception("Value not exist");
                    case SpeedType.Multiplier:
                        return ActionJToken["bpmMultiplier"]?
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
                        ActionJToken["beatsPerMinute"] = value;
                        break;
                    case SpeedType.Multiplier:
                        ActionJToken["bpmMultiplier"] = value;
                        break;
                    case SpeedType.NotAvailable:
                        goto case default;
                    default:
                        throw new Exception("Value not exist");
                }
            }
        }

        internal SetSpeed(JToken actionJToken) : base(actionJToken) { }

        public void SetValue(SpeedType speedType, float value)
        {
            SpeedType = speedType;
            Value = value;
            OnActionChanged(CacheValue.Bpm);
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
