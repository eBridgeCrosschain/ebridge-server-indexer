using AeFinder.Sdk.Entities;
using Nest;

namespace EbridgeServerIndexer.Entities;

public class PoolLiquidityRecordIndex :  AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string ChainId { get; set; }
    [Keyword] public string TokenSymbol { get; set; }
    public long Liquidity { get; set; }
    public LiquidityType LiquidityType { get; set; }
    public DateTime UpdateTime { get; set; }
    
}