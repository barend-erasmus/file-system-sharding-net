using FileSystemShardingNet.Configuration;

namespace FileSystemShardingNet.Interfaces
{
    public interface IShardingStrategyFactory
    {
        IShardingStrategy Create(ClientConfiguration configuration);
    }
}
