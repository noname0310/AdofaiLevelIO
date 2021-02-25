using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NoName.AdofaiLevelIO.Model.Actions;

namespace NoName.AdofaiLevelIO.Model.Data
{
    public class SetSpeed : Action
    {
        [JsonProperty("speedType", Order = 2)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SpeedType SpeedType { get; }

        [JsonProperty("beatsPerMinute", Order = 3)]
        public float BeatsPerMinute { get; }
        [JsonProperty("bpmMultiplier", Order = 4)]
        public float BpmMultiplier { get; }

        public SetSpeed(SpeedType speedType, float value) : base(EventType.SetSpeed)
        {
            SpeedType = speedType;
            switch (speedType)
            {
                case SpeedType.Bpm:
                    BeatsPerMinute = value;
                    BpmMultiplier = 1.0f;
                    break;
                case SpeedType.Multiplier:
                    BeatsPerMinute = 100;
                    BpmMultiplier = value;
                    break;
                case SpeedType.NotAvailable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(speedType), speedType, null);
            }
        }
    }
}