using FileSystemShardingNet.Configuration;
using FileSystemShardingNet.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemShardingNet.Tests.Configuration
{
    [TestClass]
    public class ClientConfigurationTests
    {
        private static string YAML_CONFIG = @"FileSystemType: Disk
ShardingStrategy: DJBD2
Shards:
- Master:
    Path: C:\NodeDirectories\Node-0
  Slaves:
  - Path: C:\NodeDirectories\Node-0 - Slave-0
  - Path: C:\NodeDirectories\Node-0 - Slave-1
  Slot: 682
- Master:
    Path: C:\NodeDirectories\Node-1
  Slaves:
  - Path: C:\NodeDirectories\Node-1 - Slave-0
  - Path: C:\NodeDirectories\Node-1 - Slave-1
  Slot: 1364
- Master:
    Path: C:\NodeDirectories\Node-2
  Slaves:
  - Path: C:\NodeDirectories\Node-2 - Slave-0
  - Path: C:\NodeDirectories\Node-2 - Slave-1
  Slot: 2046
- Master:
    Path: C:\NodeDirectories\Node-3
  Slaves:
  - Path: C:\NodeDirectories\Node-3 - Slave-0
  - Path: C:\NodeDirectories\Node-3 - Slave-1
  Slot: 2728
- Master:
    Path: C:\NodeDirectories\Node-4
  Slaves:
  - Path: C:\NodeDirectories\Node-4 - Slave-0
  - Path: C:\NodeDirectories\Node-4 - Slave-1
  Slot: 3410
- Master:
    Path: C:\NodeDirectories\Node-5
  Slaves:
  - Path: C:\NodeDirectories\Node-5 - Slave-0
  - Path: C:\NodeDirectories\Node-5 - Slave-1
  Slot: 4092
";

        [TestMethod]
        public void FromString_Should_Parse_FileSystemType()
        {
            var clientConfiguration = ClientConfiguration.FromString(YAML_CONFIG);

            Assert.AreEqual(FileSystemType.Disk, clientConfiguration.FileSystemType);
        }

        [TestMethod]
        public void FromString_Should_Parse_ShardingStrategy()
        {
            var clientConfiguration = ClientConfiguration.FromString(YAML_CONFIG);

            Assert.AreEqual(ShardingStrategy.DJBD2, clientConfiguration.ShardingStrategy);
        }

        // TODO: Add Tests for Validate

    }
}
