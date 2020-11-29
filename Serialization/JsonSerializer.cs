using Newtonsoft.Json;

namespace Serialization
{
    public class JsonSerializer : SerializerBase
    {
        public override string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public override T Deserialize<T>(string objStr)
        {
            return JsonConvert.DeserializeObject<T>(objStr);
        }
    }
}