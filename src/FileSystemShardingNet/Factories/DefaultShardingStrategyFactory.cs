using FileSystemShardingNet.Configuration;
using FileSystemShardingNet.Interfaces;
using FileSystemShardingNet.ShardingStrategies;

namespace FileSystemShardingNet.Factories
{
    public class DefaultShardingStrategyFactory : IShardingStrategyFactory
    {
        public IShardingStrategy Create(ClientConfiguration configuration)
        {
            switch(configuration.ShardingStrategy)
            {
                case Enums.ShardingStrategy.DJBD2:
                    return new DJBD2();
                default:
                    return null;
            }
        }
    }
}
