using System;
using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model
{
    public class Floor : JObjectMaterializer
    {
        public int Index { get; }

        public int Angle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public int Bpm
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public ActionContainer Actions { get; }

        private readonly FloorContainer _floorContainer;

        internal Floor(JObject jObject, int floorIndex, FloorContainer floorContainer) : base(jObject)
        {
            Actions = new ActionContainer(jObject, floorIndex);
            Index = floorIndex;
            _floorContainer = floorContainer;
        }
    }
}
