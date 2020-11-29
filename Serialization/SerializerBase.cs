namespace Serialization
{
    public abstract class SerializerBase
    {
        public abstract string Serialize<T>(T obj);
        public abstract T Deserialize<T>(string objStr);
    }
}