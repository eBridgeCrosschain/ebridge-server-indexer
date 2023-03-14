using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace CrossChainServer.Indexer.Entities;

public class OracleQueryInfoIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword]
    public string QueryId { get; set; }
    [Keyword]
    public string Option { get; set; }
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