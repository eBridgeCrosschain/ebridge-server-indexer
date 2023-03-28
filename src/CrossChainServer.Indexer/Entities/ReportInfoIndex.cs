using AElf.Indexing.Elasticsearch;
using Nest;

namespace CrossChainServer.Indexer.Entities;

public class ReportInfoIndex : CrossChainIndexerEntity<string>, IIndexBuild
{
    public long RoundId { get; set; }
    [Keyword]
    public string Token { get; set; }
    [Keyword]
    public string TargetChainId { get; set; }
    [Keyword]
    public string ReceiptId { get; set; }
    [Keyword]
    public string ReceiptHash { get; set; }
    public ReportStep Step { get; set; }
}

public enum ReportStep
{
    Proposed = 0,
    Confirmed = 1
}