using FileSystemShardingNet.Enums;

namespace FileSystemShardingNet.Interfaces
{
    public interface IFileSystemFactory
    {
        IFileSystemReadable CreateReadable(FileSystemType type);

        IFileSystemWriteable CreateWriteable(FileSystemType type);
    }
}
