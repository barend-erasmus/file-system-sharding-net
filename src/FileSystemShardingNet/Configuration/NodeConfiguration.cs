using System;

namespace FileSystemShardingNet.Configuration
{
    public class NodeConfiguration
    {
        public string Path { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                throw new ArgumentNullException("'Path' is null, empty or white space");
            }
        }
    }
}
