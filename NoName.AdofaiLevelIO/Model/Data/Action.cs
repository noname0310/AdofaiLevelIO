using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NoName.AdofaiLevelIO.Model.Actions;

namespace NoName.AdofaiLevelIO.Model.Data
{
    public class Action
    {
        [JsonProperty("floor", Order = 0)]
        public int Floor { get; set; }

        [JsonProperty("eventType", Order = 1)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType EventType { get; }

        public Action(EventType eventType) => EventType = eventType;
    }
}