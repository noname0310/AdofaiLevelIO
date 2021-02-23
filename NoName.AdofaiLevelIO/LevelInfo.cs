using System;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    public class LevelInfo : JObjectMaterializer
    {
        public string Artist
        {
            get => LevelReader.GetSettings(JObject)["artist"]?.ToString();
            set => LevelReader.GetSettings(JObject)["artist"] = value;
        }
        public string Song
        {
            get => LevelReader.GetSettings(JObject)["song"]?.ToString();
            set => LevelReader.GetSettings(JObject)["song"] = value;
        }
        public string Author
        {
            get => LevelReader.GetSettings(JObject)["author"]?.ToString();
            set => LevelReader.GetSettings(JObject)["author"] = value;
        }
        public float Bpm
        {
            get => LevelReader.GetSettings(JObject)["bpm"]?.ToObject<float>() ?? 
                   throw new Exception("bpm is not 32bit floating point");
            set => LevelReader.GetSettings(JObject)["bpm"] = value;
        }

        internal LevelInfo(JObject jObject) : base(jObject) { }
    }
}
