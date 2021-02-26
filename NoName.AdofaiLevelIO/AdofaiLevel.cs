using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    public class AdofaiLevel : JObjectMaterializer
    {
        /// <summary>
        /// it's unsafe feature when you have floor instance
        /// </summary>
        public JObject RawData => JObject;
        public FloorContainer Floors { get; }

        public LevelInfo LevelInfo { get; }

        private readonly string _path;

        public AdofaiLevel(string path) : base(FixParse(File.ReadAllText(path)))
        {
            _path = path;
            Floors = new FloorContainer(RawData, this);
            LevelInfo = new LevelInfo(RawData);
        }

        public void SaveLevel() => File.WriteAllText(_path, RawData.ToString());

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
                        stringBuilder.AppendLine((1 <= item.Length && item[item.Length -1] == '\r') 
                            ? item.Substring(0, item.Length - 1) : item);
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
                fixedjson[e.LineNumber - 1] = $"{str.Substring(0, e.LinePosition - 1)},{str.Substring(e.LinePosition - 1)}";
                goto parseStart;
            }
            return jObject;
        }
    }
}
