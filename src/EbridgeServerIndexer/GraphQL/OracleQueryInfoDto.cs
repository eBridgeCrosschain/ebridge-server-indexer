using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.GraphQL;

public class OracleQueryInfoDto : GraphQLDto
{
    public string QueryId { get; set; }
    public string ReceiptHash { get; set; }
    public long StartIndex { get; set; }
    public long EndIndex { get; set; }
    public OracleStep Step { get; set; }
}