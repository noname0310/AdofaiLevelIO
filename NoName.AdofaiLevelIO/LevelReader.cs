using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    internal static class LevelReader
    {
        public static string GetPathData(JObject jObject) => jObject["pathData"].ToString();
        public static JToken GetSettings(JObject jObject) => jObject["settings"];
        public static JArray GetActions(JObject jObject) => jObject["actions"] as JArray;
        public static List<JToken> GetFloorActions(JObject jObject, int floorIndex) =>
            GetActions(jObject).Where(item => item["floor"].ToObject<int>() == floorIndex).ToList();
    }
}
