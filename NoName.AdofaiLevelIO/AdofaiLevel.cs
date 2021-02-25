using System.IO;
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

        public AdofaiLevel(string path) : base(JObject.Parse(LevelFixer.Fix(File.ReadAllText(path))))
        {
            _path = path;
            Floors = new FloorContainer(RawData, this);
            LevelInfo = new LevelInfo(RawData);
        }

        public void SaveLevel() => File.WriteAllText(_path, RawData.ToString());

        public void ResetCache() => Floors.ResetCache();
    }
}
