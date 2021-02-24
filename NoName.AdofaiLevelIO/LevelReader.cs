using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    internal static class LevelReader
    {
        public static string GetPathData(JObject jObject) => jObject["pathData"].ToString();
        public static void SetPathData(JObject jObject, string data) => jObject["pathData"] = data;
        public static JToken GetSettings(JObject jObject) => jObject["settings"];
        public static JArray GetActions(JObject jObject) => jObject["actions"] as JArray;
        public static List<JToken> GetFloorActions(JObject jObject, int floorIndex) =>
            GetActions(jObject).Where(item => item["floor"].ToObject<int>() == floorIndex).ToList();

        public static void AddAction(JObject jObject, JToken jToken) => (jObject["actions"] as JArray)?.Add(jToken);
        public static void RemoveAction(JObject jObject, int index) => (jObject["actions"] as JArray)?.RemoveAt(index);
    }
}
