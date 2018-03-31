using FileSystemShardingNet.Configuration;
using FileSystemShardingNet.Enums;
using FileSystemShardingNet.FileSystems;
using FileSystemShardingNet.Interfaces;

namespace FileSystemShardingNet.Factories
{
    public class DefaultFileSystemFactory : IFileSystemFactory
    {
        public IFileSystemReadable CreateReadable(ClientConfiguration configuration)
        {
            switch(configuration.FileSystemType)
            {
                case FileSystemType.Disk:
                    return new DiskFileSystem();
                default:
                    return null;
            }
        }

        public IFileSystemWriteable CreateWriteable(ClientConfiguration configuration)
        {
            switch (configuration.FileSystemType)
            {
                case FileSystemType.Disk:
                    return new DiskFileSystem();
                default:
                    return null;
            }
        }
    }
}
