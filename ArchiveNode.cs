using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Neo.Network.P2P.Payloads;
using Neo.Persistence;
using RocksDbSharp;

namespace Neo.Plugins
{

    public partial class ArchiveNode : Plugin, IPersistencePlugin
    {
        public override string Name => "ArchiveNode";
        public override string Description => "Enables archived functions for the node";
        public NeoSystem system;

        private Settings settings;
        private static readonly Dictionary<uint, RpcServer> servers = new();
        private static readonly Dictionary<uint, List<object>> handlers = new();

        public override void Dispose()
        {
        }

        protected override void OnSystemLoaded(NeoSystem system)
        {
            this.system = system;
            RpcServerPlugin.RegisterMethods(this, system.Settings.Network);
        }

        void IPersistencePlugin.OnCommit(NeoSystem system, Block block, DataCache snapshot)
        {
            object store = typeof(SnapshotCache).GetField("store", BindingFlags.NonPublic | BindingFlags.Instance).GetValue((SnapshotCache)snapshot);
            object db = typeof(Snapshot).GetField("db", BindingFlags.NonPublic | BindingFlags.Instance).GetValue((Snapshot)store);
            if ((block.Index + 1) % 100 == 0)
            {
                var cp = ((RocksDb)db).Checkpoint();
                cp.Save(System.IO.Path.Combine(
                    String.Format(this.settings.RocksDBPath, system.Settings.Network.ToString("X")),
                    (block.Index + 1).ToString()));
            }
        }
    }
}
