namespace CrossChainServer.Indexer.GraphQL;

public class QueryDto
{
    public string ChainId { get; set; }
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
    
    public static int MaxMaxResultCount { get; set; } = 1000;
    public void Validate()
    {
        if (EndBlockHeight - StartBlockHeight +1 > MaxMaxResultCount)
        {
            throw new ArgumentException("The maximum request height range was exceeded.");
        }
    }
}