using FileSystemShardingNet.Enums;

namespace FileSystemShardingNet.Configuration
{
    public class ClientConfiguration
    {
        public ShardingStrategy ShardingStrategy { get; set; }

        public ShardConfiguration[] Shards { get; set; }

    }
}
