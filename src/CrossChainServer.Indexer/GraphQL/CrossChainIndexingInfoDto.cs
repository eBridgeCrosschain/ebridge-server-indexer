namespace CrossChainServer.Indexer.GraphQL;

public class CrossChainIndexingInfoDto : GraphQLDto
{
    public DateTime BlockTime { get; set; }
    public string IndexChainId { get; set; }
    public long IndexBlockHeight { get; set; }
}