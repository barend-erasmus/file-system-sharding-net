using FileSystemShardingNet.Interfaces;
using System.IO;

namespace FileSystemShardingNet.FileSystems
{
    public class DiskFileSystem : IFileSystemReadable, IFileSystemWriteable
    {
        public Stream GetReadStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public Stream GetWriteStream(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

            return fileStream;
        }
    }
}
