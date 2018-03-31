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
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string nodeDirectoriesPath = Path.Combine(currentPath, "NodeDirectories");

            if (Directory.Exists(nodeDirectoriesPath))
            {
                ClearDirectory(nodeDirectoriesPath);
            } else
            {
                Directory.CreateDirectory(nodeDirectoriesPath);
            }

            _clientConfiguration = BuildClientConfiguration(nodeDirectoriesPath, 6, 2);

            foreach (ShardConfiguration shardConfiguration in _clientConfiguration.Shards)
            {
                Directory.CreateDirectory(shardConfiguration.Master.Path);

                foreach (NodeConfiguration slaveNodeConfiguration in shardConfiguration.Slaves)
                {
                    Directory.CreateDirectory(slaveNodeConfiguration.Path);
                }
            }

            _fileSystemShardingNetClient = new FileSystemShardingNetClient(_clientConfiguration);
        }

        [TestCleanup]
        public void Cleanup()
        {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string nodeDirectoriesPath = Path.Combine(currentPath, "NodeDirectories");

            if (Directory.Exists(nodeDirectoriesPath))
            {
                ClearDirectory(nodeDirectoriesPath);
            }
        }

        [TestMethod]
        public void Write()
        {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string testFilesPath = Path.Combine(currentPath, "TestFiles");

            foreach (FileInfo fileInfo in new DirectoryInfo(testFilesPath).GetFiles())
            {
                FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);

                string path = AbsolutePathToRelativePath(fileInfo.FullName, $@"{testFilesPath}\");

                _fileSystemShardingNetClient.Write(path, fileStream);

                fileStream.Close();
                fileStream.Dispose();
            }
        }

        private string AbsolutePathToRelativePath(string path, string referencePath)
        {
            Uri uri = new Uri(path);

            Uri referenceUri = new Uri(referencePath);

            return referenceUri.MakeRelativeUri(uri).ToString();
        }

        private ClientConfiguration BuildClientConfiguration(string nodeDirectoriesPath, int numberOfShards, int numberOfSlaves)
        {
            ClientConfiguration clientConfiguration = new ClientConfiguration()
            {
                FileSystemType = FileSystemType.Disk,
                ShardingStrategy = ShardingStrategy.DJBD2,
                Shards = new ShardConfiguration[numberOfShards],
            };

            for (int shardIndex = 0; shardIndex < numberOfShards; shardIndex++)
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

                for (int slaveIndex = 0; slaveIndex < numberOfSlaves; slaveIndex++)
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
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                subDirectoryInfo.Delete(true);
            }
        }

        private Stream StringToStream(string str)
        {
            Stream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);

            writer.Write(str);

            writer.Flush();

            stream.Position = 0;

            return stream;
        }
    }
}
