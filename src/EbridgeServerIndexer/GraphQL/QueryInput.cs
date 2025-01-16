namespace EbridgeServerIndexer.GraphQL;

public class QueryInput
{
    public string ChainId { get; set; }
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
    
    public int MaxMaxResultCount { get; set; } = 1000;
    public void Validate()
    {
        if (EndBlockHeight - StartBlockHeight +1 > MaxMaxResultCount)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxMaxResultCount),
                $"Max allowed value for {nameof(MaxMaxResultCount)} is {MaxMaxResultCount}.");
        }
    }
}