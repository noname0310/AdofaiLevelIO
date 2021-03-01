using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    internal static class LevelReader
    {
        internal static JArray GetActions(JObject jObject) => jObject["actions"] as JArray;
        internal static IEnumerable<JToken> GetFloorActions(JObject jObject, int floorIndex) =>
            GetActions(jObject).Where(item => item["floor"].ToObject<int>() == floorIndex);
    }
}
