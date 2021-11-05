using Microsoft.Extensions.Configuration;
using Neo.SmartContract.Native;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Neo.Plugins
{
    class Settings
    {
        public string RocksDBPath { get; }
        public uint ArchiveSkipRate { get; }
        public static Settings Default { get; private set; }

        public Settings(IConfigurationSection section)
        {
            this.RocksDBPath = section.GetValue("RocksDBPath", "Data_RocksDB_{0}");
            this.ArchiveSkipRate = section.GetValue("ArchiveSkipRate", 100u);
        }
    }
}
