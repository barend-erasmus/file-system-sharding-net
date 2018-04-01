using FileSystemShardingNet.ShardingStrategies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemShardingNet.Tests.ShardingStrategies
{
    [TestClass]
    public class DJBD2Tests
    {
        [TestMethod]
        public void FromString_Should_Parse_FileSystemType()
        {
            var shardingStrategy = new DJBD2();

            var slot = shardingStrategy.GetSlot(4096, "my-file.txt");

            Assert.AreEqual(3270, slot);
        }
    }
}
