using Newtonsoft.Json.Linq;

namespace NoName.AdofaiLevelIO
{
    public class JObjectMaterializer
    {
        protected readonly JObject JObject;

        public JObjectMaterializer(JObject jObject) =>  JObject = jObject;
    }
}
