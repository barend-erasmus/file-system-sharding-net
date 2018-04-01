# File System Sharding .NET

Application Layer File System Sharding Client 

## Installation

**Package Manager**

`Install-Package FileSystemSharding.NET -Version 1.0.0`

**.NET CLI**

`dotnet add package FileSystemSharding.NET --version 1.0.0`

**Paket CLI**

`paket add FileSystemSharding.NET --version 1.0.0`

## Usage

### Read

```CSharp
var config = File.ReadAllText("file-system-sharding-client.yaml");
var clientConfiguration = ClientConfiguration.FromString(config);

var fileSystemShardingNetClient = new FileSystemShardingNetClient(_clientConfiguration);

var stream = fileSystemShardingNetClient.ReadFromMaster("my-file.txt");

// OR

var stream = fileSystemShardingNetClient.ReadFromSlave("my-file.txt", 0);

// TODO: Use stream here...

stream.Close();
stream.Dispose();
```

## Write

```CSharp
var config = File.ReadAllText("file-system-sharding-client.yaml");
var clientConfiguration = ClientConfiguration.FromString(config);

var fileSystemShardingNetClient = new FileSystemShardingNetClient(_clientConfiguration);

var fileStream = new FileStream("my-file.txt", FileMode.Open, FileAccess.Read);

fileSystemShardingNetClient.Write("my-file.txt", fileStream);

fileStream.Close();
fileStream.Dispose();
```

## Configuration

### Options

* FileSystemType: `Disk` | `Other`
* ShardingStrategy: `DJBD2` | `FNV1A`
* Shards
    * Master
        * Path: Directory where files should be stored for this node
        * Slaves:
            * Path: Directory where files should be stored for this node
    * Slot: Slot number ranging from `0` to `4092`

### Example

`file-system-sharding-client.yaml`
```yaml
FileSystemType: Disk
ShardingStrategy: DJBD2
Shards:
- Master:
    Path: C:\NodeDirectories\Node-0
  Slaves:
  - Path: C:\NodeDirectories\Node-0 - Slave-0
  - Path: C:\NodeDirectories\Node-0 - Slave-1
  Slot: 682
- Master:
    Path: C:\NodeDirectories\Node-1
  Slaves:
  - Path: C:\NodeDirectories\Node-1 - Slave-0
  - Path: C:\NodeDirectories\Node-1 - Slave-1
  Slot: 1364
- Master:
    Path: C:\NodeDirectories\Node-2
  Slaves:
  - Path: C:\NodeDirectories\Node-2 - Slave-0
  - Path: C:\NodeDirectories\Node-2 - Slave-1
  Slot: 2046
- Master:
    Path: C:\NodeDirectories\Node-3
  Slaves:
  - Path: C:\NodeDirectories\Node-3 - Slave-0
  - Path: C:\NodeDirectories\Node-3 - Slave-1
  Slot: 2728
- Master:
    Path: C:\NodeDirectories\Node-4
  Slaves:
  - Path: C:\NodeDirectories\Node-4 - Slave-0
  - Path: C:\NodeDirectories\Node-4 - Slave-1
  Slot: 3410
- Master:
    Path: C:\NodeDirectories\Node-5
  Slaves:
  - Path: C:\NodeDirectories\Node-5 - Slave-0
  - Path: C:\NodeDirectories\Node-5 - Slave-1
  Slot: 4092
```