using System.IO;

namespace FileSystemShardingNet.Interfaces
{
    public interface IFileSystemReadable
    {
        Stream GetReadStream(string path);
    }
}
