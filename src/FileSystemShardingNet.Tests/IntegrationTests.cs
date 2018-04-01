using FileSystemShardingNet.Configuration;
using FileSystemShardingNet.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace FileSystemShardingNet.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private ClientConfiguration _clientConfiguration;

        private FileSystemShardingNetClient _fileSystemShardingNetClient;

        [TestInitialize]
        public void Initialize()
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var nodeDirectoriesPath = Path.Combine(currentPath, "NodeDirectories");

            if (Directory.Exists(nodeDirectoriesPath))
            {
                ClearDirectory(nodeDirectoriesPath);
            } else
            {
                Directory.CreateDirectory(nodeDirectoriesPath);
            }

            _clientConfiguration = BuildClientConfiguration(nodeDirectoriesPath, 6, 2);

            foreach (var shardConfiguration in _clientConfiguration.Shards)
            {
                Directory.CreateDirectory(shardConfiguration.Master.Path);

                foreach (var slaveNodeConfiguration in shardConfiguration.Slaves)
                {
                    Directory.CreateDirectory(slaveNodeConfiguration.Path);
                }
            }

            _fileSystemShardingNetClient = new FileSystemShardingNetClient(_clientConfiguration);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var nodeDirectoriesPath = Path.Combine(currentPath, "NodeDirectories");

            if (Directory.Exists(nodeDirectoriesPath))
            {
                ClearDirectory(nodeDirectoriesPath);
            }
        }

        [TestMethod]
        public void Write()
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var testFilesPath = Path.Combine(currentPath, "TestFiles");

            foreach (var fileInfo in new DirectoryInfo(testFilesPath).GetFiles())
            {
                var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);

                var path = AbsolutePathToRelativePath(fileInfo.FullName, $@"{testFilesPath}\");

                _fileSystemShardingNetClient.Write(path, fileStream);

                fileStream.Close();
                fileStream.Dispose();
            }
        }

        private string AbsolutePathToRelativePath(string path, string referencePath)
        {
            var uri = new Uri(path);

            var referenceUri = new Uri(referencePath);

            return referenceUri.MakeRelativeUri(uri).ToString();
        }

        private ClientConfiguration BuildClientConfiguration(string nodeDirectoriesPath, int numberOfShards, int numberOfSlaves)
        {
            var clientConfiguration = new ClientConfiguration()
            {
                FileSystemType = FileSystemType.Disk,
                ShardingStrategy = ShardingStrategy.DJBD2,
                Shards = new ShardConfiguration[numberOfShards],
            };

            for (var shardIndex = 0; shardIndex < numberOfShards; shardIndex++)
            {
                clientConfiguration.Shards[shardIndex] = new ShardConfiguration()
                {
                    Master = new NodeConfiguration()
                    {
                        Path = Path.Combine(nodeDirectoriesPath, $"Node-{shardIndex}"),
                    },
                    Slaves = new NodeConfiguration[numberOfSlaves],
                    Slot = 4096 / numberOfShards * (shardIndex + 1),
                };

                for (var slaveIndex = 0; slaveIndex < numberOfSlaves; slaveIndex++)
                {

                    clientConfiguration.Shards[shardIndex].Slaves[slaveIndex] = new NodeConfiguration()
                    {
                        Path = Path.Combine(nodeDirectoriesPath, $"Node-{shardIndex} - Slave-{slaveIndex}")
                    };
                }
            }

            return clientConfiguration;
        }

        private void ClearDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                subDirectoryInfo.Delete(true);
            }
        }

        private Stream StringToStream(string str)
        {
            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            writer.Write(str);

            writer.Flush();

            stream.Position = 0;

            return stream;
        }
    }
}
