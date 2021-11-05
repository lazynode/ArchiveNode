# ArchiveNode

ArchiveNode is a plugin for neo-node which provides archive functions. It supports queries upon any function at any block height.

## Dependency

ArchiveNode depends on `RocksDBStore` because rocksdb supports checkpoint efficiently. Therefore, the database must be RocksDBStore.

ArchiveNode also depends on `RpcServer` because rpc functions `invokearchivefunction` and `invokearchivescript` are provided through it.

You can find them [here](https://github.com/neo-project/neo-modules).

## Install

1. install neo-node first, set the `config.json`'s `Storage` param as `{"Engine":"RocksDBStore","Path": "Data_RocksDB_{0}"}` 
2. in your neo-node directory, create a folder `Plugins` if not exist
3. install dependencies `RocksDBStore.dll` and `RocksDbSharp.dll` as well as `RpcServer.dll` to `Plugins`
4. copy `RpcServer` config folder to `Plugins`, config the `RpcServer/config.json`, set `network` the same as your neo-node's config
5. install our `ArchiveNode.dll` to `Plugins`
6. copy `ArchiveNode` config folder to `Plugins`, config the `ArchiveNode/config.json`, set `RocksDBPath` the same as your neo-node's config
7. config the `ArchiveNode/config.json`, set `ArchiveSkipRate` depends on you need. If it is set to `1`, then every block will be archived and can be used by client. If it is set to `100`, then neo-node will only archive 1 block every 100 blocks. The disk usage should be considered, too. If every block is archived, the disk consumption will be significantly large.
8. start the node and check if block is synced

## Client Usage

`invokearchivefunction` is the same as `invokefunction` except it accept one more params which indicate the block height you want to query.

`invokearchivescript` is the same as `invokescript` except it accept one more params which indicate the block height you want to query.

You can check the `invokefunction`'s usage [here](https://docs.neo.org/docs/en-us/reference/rpc/latest-version/api/invokefunction.html).

```
curl -sd '{
    "jsonrpc": "2.0",
    "method": "invokearchivefunction",
    "params": ["0xd2a4cff31913016155e38e474a2c06d08be276cf","totalSupply",[],[
      {
        "account": "NTpqYncLsNNsMco71d9qrd5AWXdCq8YLAA",
        "scopes": "CalledByEntry"
      }
  ],"5000"],
    "id": 1
}' http://127.0.0.1:10332
```