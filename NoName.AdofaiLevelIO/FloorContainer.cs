using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model;

namespace NoName.AdofaiLevelIO
{
    public class FloorContainer : JObjectMaterializer, IEnumerable<Floor>
    {
        public int Count => LevelReader.GetPathData(JObject).Length + 1;

        public Floor this[int index]
        {
            get
            {
                if (Count <= index)
                    throw new IndexOutOfRangeException();
                return new Floor(JObject, index, _adofaiLevel, _floorCache);
            }
        }

        private readonly AdofaiLevel _adofaiLevel;
        private readonly FloorCacheContainer _floorCache;

        internal FloorContainer(JObject jObject, AdofaiLevel adofaiLevel) : base(jObject)
        {
            _adofaiLevel = adofaiLevel;
            _floorCache = new FloorCacheContainer();
        }

        public void ResetCache() => _floorCache.Clear();

        public IEnumerator<Floor> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
