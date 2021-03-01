using System;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    public class LevelInfo
    {
        public string Artist
        {
            get => _settingsJToken["artist"]?.ToString();
            set => _settingsJToken["artist"] = value;
        }
        public string Song
        {
            get => _settingsJToken["song"]?.ToString();
            set => _settingsJToken["song"] = value;
        }
        public string Author
        {
            get => _settingsJToken["author"]?.ToString();
            set => _settingsJToken["author"] = value;
        }
        public float Bpm
        {
            get => _settingsJToken["bpm"]?.ToObject<float>() ?? 
                   throw new Exception("bpm is not 32bit floating point");
            set => _settingsJToken["bpm"] = value;
        }

        private readonly JToken _settingsJToken;

        internal LevelInfo(JToken settingsJToken) => _settingsJToken = settingsJToken;
    }
}
