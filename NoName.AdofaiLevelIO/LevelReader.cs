using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    internal static class LevelReader
    {
        internal static string GetPathData(JObject jObject) => jObject["pathData"].ToString();
        internal static void SetPathData(JObject jObject, string data) => jObject["pathData"] = data;
        internal static JToken GetSettings(JObject jObject) => jObject["settings"];
        internal static JArray GetActions(JObject jObject) => jObject["actions"] as JArray;
        internal static List<JToken> GetFloorActions(JObject jObject, int floorIndex) =>
            GetActions(jObject).Where(item => item["floor"].ToObject<int>() == floorIndex).ToList();

        internal static void AddAction(JObject jObject, JToken jToken) => (jObject["actions"] as JArray)?.Add(jToken);
        internal static void RemoveAction(JObject jObject, int index) => (jObject["actions"] as JArray)?.RemoveAt(index);
    }
}
