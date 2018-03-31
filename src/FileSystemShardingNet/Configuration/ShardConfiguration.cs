namespace FileSystemShardingNet.Configuration
{
    public class ShardConfiguration
    {
        public NodeConfiguration Master { get; set; }

        public NodeConfiguration[] Slaves { get; set; }

        public int Slot { get; set; }
    }
}
