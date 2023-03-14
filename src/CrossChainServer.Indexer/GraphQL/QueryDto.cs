namespace CrossChainServer.Indexer.GraphQL;

public class QueryDto
{
    public string ChainId { get; set; }
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
}