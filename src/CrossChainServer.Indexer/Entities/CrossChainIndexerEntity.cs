using AElfIndexer.Client;

namespace CrossChainServer.Indexer.Entities;

public class CrossChainIndexerEntity<T> : AElfIndexerClientEntity<T>
{
    public DateTime BlockTime { get; set; }
}