namespace FileSystemShardingNet.Interfaces
{
    public interface IShardingStrategy
    {
        int GetSlot(int numberOfSlots, string path);
    }
}
