using FileSystemShardingNet.Interfaces;

namespace FileSystemShardingNet.ShardingStrategies
{
    public class DJBD2 : IShardingStrategy
    {
        public int GetSlot(int numberOfSlots, string path)
        {
            uint hash = ComputeHash(path);

            return (int)(hash % numberOfSlots);
        }

        private uint ComputeHash(string str)
        {
            uint hash = 0x1505;

            for (int index = 0; index < str.Length; index++)
            {
                hash = ((hash << 5) + hash) + str[index];
            }

            return hash;
        }
    }
}
