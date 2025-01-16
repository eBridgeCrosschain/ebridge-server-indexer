using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.GraphQL;

public class UserLiquidityRecordDto : GraphQLDto
{
    public string Provider { get; set; }
    public string TokenSymbol { get; set; }
    public long Liquidity { get; set; }
    public LiquidityType LiquidityType { get; set; }
    public DateTime UpdateTime { get; set; }
}