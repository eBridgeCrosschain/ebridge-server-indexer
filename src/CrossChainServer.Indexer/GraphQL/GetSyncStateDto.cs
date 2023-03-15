using AElfIndexer;

namespace CrossChainServer.Indexer.GraphQL;

public class GetSyncStateDto
{
    public string ChainId { get; set; }
    public BlockFilterType FilterType { get; set; }
}