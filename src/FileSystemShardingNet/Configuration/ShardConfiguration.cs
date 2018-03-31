using System;

namespace FileSystemShardingNet.Configuration
{
    public class ShardConfiguration
    {
        public NodeConfiguration Master { get; set; }

        public NodeConfiguration[] Slaves { get; set; }

        public int Slot { get; set; }

        public void Validate()
        {
            if (Master == null)
            {
                throw new ArgumentNullException("'Master' is not set");
            }

            Master.Validate();

            if (Slaves == null)
            {
                throw new ArgumentNullException("'Slaves' is not set");
            }

            foreach (NodeConfiguration nodeConfiguration in Slaves)
            {
                nodeConfiguration.Validate();
            }
        }
    }
}
