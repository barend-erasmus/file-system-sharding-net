using FileSystemShardingNet.Interfaces;

namespace FileSystemShardingNet.ShardingStrategies
{
    public class DJBD2 : IShardingStrategy
    {
        public int GetSlot(int numberOfSlots, string path)
        {
            var hash = ComputeHash(path);

            return (int)(hash % numberOfSlots);
        }

        protected uint ComputeHash(string str)
        {
            uint hash = 0x1505;

            for (var index = 0; index < str.Length; index++)
            {
                hash = ((hash << 5) + hash) + str[index];
            }

            return hash;
        }
    }
}
