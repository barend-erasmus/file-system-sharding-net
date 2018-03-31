using FileSystemShardingNet.Configuration;

namespace FileSystemShardingNet.Interfaces
{
    public interface IFileSystemFactory
    {
        IFileSystemReadable CreateReadable(ClientConfiguration configuration);

        IFileSystemWriteable CreateWriteable(ClientConfiguration configuration);
    }
}
