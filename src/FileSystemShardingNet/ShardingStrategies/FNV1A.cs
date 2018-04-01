using FileSystemShardingNet.Interfaces;

namespace FileSystemShardingNet.ShardingStrategies
{
    public class FNV1A : IShardingStrategy
    {
        public int GetSlot(int numberOfSlots, string path)
        {
            var hash = ComputeHash(path);

            return (int)(hash % numberOfSlots);
        }

        private uint ComputeHash(string str)
        {
            uint hash = 0x811c9dc5;

            for (var index = 0; index < str.Length; index++)
            {
                hash ^= str[index];
                hash += (hash << 1) + (hash << 4) + (hash << 7) + (hash << 8) + (hash << 24);
            }

            return hash;
        }
    }
}
