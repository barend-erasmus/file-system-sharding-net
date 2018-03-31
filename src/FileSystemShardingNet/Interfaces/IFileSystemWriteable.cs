using System.IO;

namespace FileSystemShardingNet.Interfaces
{
    public interface IFileSystemWriteable
    {
        Stream GetWriteStream(string path);
    }
}
