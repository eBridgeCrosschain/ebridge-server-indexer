using AElfIndexer.Client;

namespace CrossChainServer.Indexer.Entities;

public class CrossChainServerIndexerEntity<T> : AElfIndexerClientEntity<T>
{
    public DateTime BlockTime { get; set; }
}