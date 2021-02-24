using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model.Actions
{
    public class Twirl : Action
    {
        internal Twirl(JObject jObject, int floorIndex, int actionIndex, FloorCacheContainer floorCacheContainer) 
            : base(jObject, floorIndex, actionIndex, floorCacheContainer) { }
    }
}
