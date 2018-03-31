using FileSystemShardingNet.Configuration;
using FileSystemShardingNet.Factories;
using FileSystemShardingNet.Interfaces;
using System;
using System.IO;

namespace FileSystemShardingNet
{
    public class FileSystemShardingNetClient
    {
        private readonly ClientConfiguration _configuration;

        private IFileSystemReadable _fileSystemReadable;

        private IFileSystemWriteable _fileSystemWriteable;

        private const int _numberOfSlots = 4096;

        private readonly IShardingStrategy _shardingStrategy;

        public FileSystemShardingNetClient(ClientConfiguration configuration, IFileSystemFactory fileSystemFactory = null, IShardingStrategyFactory shardingStrategyFactory = null)
        {
            configuration.Validate();
            _configuration = configuration;

            fileSystemFactory = fileSystemFactory ?? new DefaultFileSystemFactory();
            SetFileSystems(fileSystemFactory);

            shardingStrategyFactory = shardingStrategyFactory ?? new DefaultShardingStrategyFactory();
            _shardingStrategy = shardingStrategyFactory.Create(configuration);
        }

        public Stream ReadFromMaster(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("'path' is null, empty or white space");
            }

            ShardConfiguration shardConfiguration = GetShardConfiguration(path);

            NodeConfiguration nodeConfiguration = shardConfiguration.Master;

            return GetStreamFromNodeConfiguration(nodeConfiguration, path);
        }

        public Stream ReadFromSlave(string path, int slaveIndex)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("'path' is null, empty or white space");
            }

            ShardConfiguration shardConfiguration = GetShardConfiguration(path);

            if (slaveIndex >= shardConfiguration.Slaves.Length)
            {
                throw new IndexOutOfRangeException($"'slaveIndex', {slaveIndex}, is not in range");
            }

            NodeConfiguration nodeConfiguration = shardConfiguration.Slaves[slaveIndex];

            return GetStreamFromNodeConfiguration(nodeConfiguration, path);
        }

        public bool Write(string path, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("'path' is null, empty or white space");
            }

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

        private void SetFileSystems(IFileSystemFactory fileSystemFactory)
        {
            _fileSystemReadable = fileSystemFactory.CreateReadable(_configuration);

            _fileSystemWriteable = fileSystemFactory.CreateWriteable(_configuration);
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
