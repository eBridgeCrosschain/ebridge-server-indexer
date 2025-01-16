using AeFinder.Sdk.Entities;
using Nest;

namespace EbridgeServerIndexer.Entities;

public class UserLiquidityRecordIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string ChainId { get; set; }
    [Keyword] public string TokenSymbol { get; set; }
    public long Liquidity { get; set; }
    [Keyword] public string Provider { get; set; }
    public LiquidityType LiquidityType { get; set; }
    public DateTime UpdateTime { get; set; }

}

public enum LiquidityType
{
    Add = 0,
    Remove = 1
}

