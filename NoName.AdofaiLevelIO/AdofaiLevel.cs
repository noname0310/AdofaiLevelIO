using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    public class AdofaiLevel
    {
        /// <summary>
        /// it's unsafe feature when you have floor instance
        /// </summary>
        public JObject RawData { get; }
        public FloorContainer Floors { get; }
        public LevelInfo LevelInfo { get; }

        private readonly string _path;

        public AdofaiLevel(string path)
        {
            RawData = FixParse(File.ReadAllText(path));
            _path = path;
            Floors = new FloorContainer(RawData["pathData"]?.ToString(), this);
            LevelInfo = new LevelInfo(RawData["settings"] ?? throw new Exception("parse error"));
        }

        public void SaveLevel()
        {
            RawData["pathData"] = Floors.PathData;

            var actionsJArray = new JArray();
            foreach (var floor in Floors)
            {
                foreach (var action in floor.Actions)
                {
                    action.ActionJToken["floor"] = floor.Index;
                    actionsJArray.Add(action.ActionJToken);
                }
            }

            RawData["actions"] = actionsJArray;
            File.WriteAllText(_path, RawData.ToString());
        }

        public void ResetCache() => Floors.ResetCache();

        private static JObject FixParse(string json)
        {
            JObject jObject;
            string[] fixedjson = null;

            parseStart:
            try
            {
                if (fixedjson != null)
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var item in fixedjson)
                        stringBuilder.AppendLine((1 <= item.Length && item[item.Length - 1] == '\r')
                            ? item.Substring(0, item.Length - 1)
                            : item);
                    jObject = JObject.Parse(stringBuilder.ToString());
                }
                else
                    jObject = JObject.Parse(json);
            }
            catch (JsonReaderException e)
            {
                if (fixedjson == null)
                    fixedjson = json.Split('\n');

                var str = fixedjson[e.LineNumber - 1];
                fixedjson[e.LineNumber - 1] =
                    $"{str.Substring(0, e.LinePosition - 1)},{str.Substring(e.LinePosition - 1)}";
                goto parseStart;
            }

            return jObject;
        }
    }
}
