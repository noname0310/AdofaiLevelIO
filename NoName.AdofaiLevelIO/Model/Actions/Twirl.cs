using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO.Model.Actions
{
    public class Twirl : Action
    {
        internal Twirl(JObject jObject, int floorIndex, int actionIndex) 
            : base(jObject, floorIndex, actionIndex) { }
    }
}
