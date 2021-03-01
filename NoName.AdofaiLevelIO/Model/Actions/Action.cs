using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model.Actions
{
    public class Action
    {
        internal event ActionContainer.ActionChanged ActionChanged;

        public EventType EventType => ParseEventType(ActionJToken["eventType"]?.ToString());
        
        internal JToken ActionJToken { get; }

        internal Action(JToken actionJToken)
        {
            ActionJToken = actionJToken;
        }

        protected internal void OnActionChanged(CacheValue cacheValue) => ActionChanged?.Invoke(cacheValue);

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
