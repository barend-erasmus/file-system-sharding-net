using FileSystemShardingNet.Interfaces;
using System.IO;

namespace FileSystemShardingNet.FileSystems
{
    public class DiskFileSystem : IFileSystemReadable, IFileSystemWriteable
    {
        public Stream GetReadStream(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            return fileStream;
        }

        public Stream GetWriteStream(string path)
        {
            var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

            return fileStream;
        }
    }
}
