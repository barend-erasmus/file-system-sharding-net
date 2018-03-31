using System.IO;

namespace FileSystemShardingNet.Interfaces
{
    public interface IFileSystemShardingNetClient
    {
        Stream ReadFromMaster(string path);

        Stream ReadFromSlave(string path, int slaveIndex);

        bool Write(string path, Stream stream);
    }
}
