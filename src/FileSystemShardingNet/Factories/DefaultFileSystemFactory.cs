using FileSystemShardingNet.Enums;
using FileSystemShardingNet.FileSystems;
using FileSystemShardingNet.Interfaces;

namespace FileSystemShardingNet.Factories
{
    public class DefaultFileSystemFactory : IFileSystemFactory
    {
        public IFileSystemReadable CreateReadable(FileSystemType type)
        {
            switch(type)
            {
                case FileSystemType.Disk:
                    return new DiskFileSystem();
                default:
                    return null;
            }
        }

        public IFileSystemWriteable CreateWriteable(FileSystemType type)
        {
            switch (type)
            {
                case FileSystemType.Disk:
                    return new DiskFileSystem();
                default:
                    return null;
            }
        }
    }
}
