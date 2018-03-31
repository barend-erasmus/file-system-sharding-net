using FileSystemShardingNet.Enums;
using System;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FileSystemShardingNet.Configuration
{
    public class ClientConfiguration
    {
        public FileSystemType FileSystemType { get; set; }

        public ShardingStrategy ShardingStrategy { get; set; }

        public ShardConfiguration[] Shards { get; set; }

        public static ClientConfiguration FromString(string str)
        {
            Deserializer deserializer = new DeserializerBuilder()
                .Build();

            ClientConfiguration clientConfiguration = deserializer.Deserialize<ClientConfiguration>(new StringReader(str));

            return clientConfiguration;
        }

        public string ToYAML()
        {
            Serializer serializer = new SerializerBuilder().Build();

            string yaml = serializer.Serialize(this);

            return yaml;
        }

        public void Validate()
        {
            if (FileSystemType == 0)
            {
                throw new ArgumentNullException("'FileSystemType' is not set");
            }

            if (ShardingStrategy == 0)
            {
                throw new ArgumentNullException("'ShardingStrategy' is not set");
            }

            if (Shards == null)
            {
                throw new ArgumentNullException("'Shards' is not set");
            }

            foreach (ShardConfiguration shard in Shards)
            {
                shard.Validate();
            }
        }
    }
}
