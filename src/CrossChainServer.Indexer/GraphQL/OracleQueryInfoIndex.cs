using CrossChainServer.Indexer.Entities;

namespace CrossChainServer.Indexer.GraphQL;

public class OracleQueryInfoDto : GraphQLDto
{
    public string QueryId { get; set; }
    public string Option { get; set; }
    public OracleStep Step { get; set; }
}