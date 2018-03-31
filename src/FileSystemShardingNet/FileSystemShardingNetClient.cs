using FileSystemShardingNet.Configuration;
using FileSystemShardingNet.Enums;
using FileSystemShardingNet.Factories;
using FileSystemShardingNet.Interfaces;
using System;
using System.IO;

namespace FileSystemShardingNet
{
    public class FileSystemShardingNetClient
    {
        private readonly ClientConfiguration _configuration;

        private readonly IFileSystemReadable _fileSystemReadable;

        private readonly IFileSystemWriteable _fileSystemWriteable;

        private const int _numberOfSlots = 4096;

        private readonly IShardingStrategy _shardingStrategy;

        public FileSystemShardingNetClient(ClientConfiguration configuration)
        {
            _configuration = configuration;

            IFileSystemFactory fileSystemFactory = new DefaultFileSystemFactory();

            _fileSystemReadable = fileSystemFactory.CreateReadable(FileSystemType.Disk);

            _fileSystemWriteable = fileSystemFactory.CreateWriteable(FileSystemType.Disk);

            IShardingStrategyFactory shardingStrategyFactory = new DefaultShardingStrategyFactory();

            _shardingStrategy = shardingStrategyFactory.Create(configuration);
        }

        public Stream Read(string path)
        {
            ShardConfiguration shardConfiguration = GetShardConfiguration(path);

            NodeConfiguration nodeConfiguration = shardConfiguration.Master;

            return GetStreamFromNodeConfiguration(nodeConfiguration, path);
        }

        public bool Write(string path, Stream stream)
        {
            ShardConfiguration shardConfiguration = GetShardConfiguration(path);

            NodeConfiguration masterNodeConfiguration = shardConfiguration.Master;

            WriteStreamToNodeConfiguration(masterNodeConfiguration, path, stream);

            foreach (NodeConfiguration nodeConfiguration in shardConfiguration.Slaves)
            {
                WriteStreamToNodeConfiguration(nodeConfiguration, path, stream);
            }

            return true;
        }

        private string BuildPath(NodeConfiguration nodeConfiguration, string path)
        {
            return Path.Combine(nodeConfiguration.Path, path);
        }

        private ShardConfiguration GetShardConfiguration(string path)
        {
            int slot = _shardingStrategy.GetSlot(_numberOfSlots, path);

            foreach (ShardConfiguration shardConfiguration in _configuration.Shards)
            {
                if (slot <= shardConfiguration.Slot)
                {
                    return shardConfiguration;
                }
            }

            return null;
        }

        private Stream GetStreamFromNodeConfiguration(NodeConfiguration nodeConfiguration, string path)
        {
            return _fileSystemReadable.GetReadStream(BuildPath(nodeConfiguration, path));
        }

        private void WriteStreamToNodeConfiguration(NodeConfiguration nodeConfiguration, string path, Stream stream)
        {
            Stream writeableStream = _fileSystemWriteable.GetWriteStream(BuildPath(nodeConfiguration, path));

            byte[] buffer = new byte[32768];

            int read;

            stream.Position = 0;

            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                writeableStream.Write(buffer, 0, read);
            }

            writeableStream.Close();
            writeableStream.Dispose();
        }

    }
}
