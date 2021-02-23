using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model;

namespace NoName.AdofaiLevelIO
{
    public class FloorContainer : JObjectMaterializer, IEnumerable<Floor>
    {
        public int Count => LevelReader.GetPathData(JObject).Length;

        public Floor this[int index]
        {
            get
            {
                if (Count <= index)
                    throw new IndexOutOfRangeException();
                return new Floor(JObject, index, this);
            }
        }

        internal FloorContainer(JObject jObject) : base(jObject) { }

        public IEnumerator<Floor> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
