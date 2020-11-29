using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization
{
    public class XmlSerializer : SerializerBase
    {
        private static ConcurrentDictionary<Type, System.Xml.Serialization.XmlSerializer> serializers = new ConcurrentDictionary<Type, System.Xml.Serialization.XmlSerializer>();

        public override string Serialize<T>(T obj)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty}); var serializer = serializers.GetOrAdd(typeof(T), x => new System.Xml.Serialization.XmlSerializer(typeof(T)));
            var settings = new XmlWriterSettings
            {
                Indent = true, OmitXmlDeclaration = true
            };
            //using
            var stream = new StringWriter();
            var writer = XmlWriter.Create(stream, settings);

            serializer.Serialize(writer, obj, emptyNamespaces);
            return stream.ToString();
        }

        public override T Deserialize<T>(string objStr)
        {
            var serializer = serializers.GetOrAdd(typeof(T), x => new System.Xml.Serialization.XmlSerializer(typeof(T)));
            //using
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(objStr));
            return (T) serializer.Deserialize(memoryStream);
        }
    }
}