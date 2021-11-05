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

        protected override void OnSystemLoaded(NeoSystem system)
        {
            this.system = system;
            this.settings = new Settings(GetConfiguration());
            RpcServerPlugin.RegisterMethods(this, system.Settings.Network);
        }

        void IPersistencePlugin.OnCommit(NeoSystem system, Block block, DataCache snapshot)
        {
            if ((block.Index + 1) % this.settings.ArchiveSkipRate == 0)
            {
                object store = snapshot.GetType().GetField("store", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(snapshot);
                object db = store.GetType().GetField("db", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(store);
                var cp = ((RocksDb)db).Checkpoint();
                cp.Save(System.IO.Path.Combine(
                    String.Format(this.settings.RocksDBPath, system.Settings.Network.ToString("X")),
                    (block.Index + 1).ToString()));
                cp.Dispose();
            }
        }
    }
}
