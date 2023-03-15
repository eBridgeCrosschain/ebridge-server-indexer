using System;
using CrossChainServer.Indexer.Entities;

namespace CrossChainServer.Indexer.GraphQL;

public class OracleQueryInfoDto : GraphQLDto
{
    public string QueryId { get; set; }
    public string ReceiptHash { get; set; }
    public long StartIndex { get; set; }
    public long EndIndex { get; set; }
    public OracleStep Step { get; set; }
    public DateTime BlockTime { get; set; }
}