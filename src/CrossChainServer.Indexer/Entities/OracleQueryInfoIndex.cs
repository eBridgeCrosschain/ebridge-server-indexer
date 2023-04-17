using AElf.Indexing.Elasticsearch;
using Nest;

namespace CrossChainServer.Indexer.Entities;

public class OracleQueryInfoIndex : CrossChainIndexerEntity<string>, IIndexBuild
{
    [Keyword]
    public string QueryId { get; set; }
    [Keyword]
    public string ReceiptHash { get; set; }
    public long StartIndex { get; set; }
    public long EndIndex { get; set; }
    public OracleStep Step { get; set; }
}

public enum OracleStep
{
    QueryCreated,
    Committed,
    SufficientCommitmentsCollected,
    CommitmentRevealed,
    QueryCompleted
}