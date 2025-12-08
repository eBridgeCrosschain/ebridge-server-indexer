using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using EbridgeServerIndexer.Entities;
using Volo.Abp.ObjectMapping;

namespace EbridgeServerIndexer.Processors.TokenPool;

public abstract class TokenPoolProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            EbridgeConst.AELF => EbridgeConst.TokenPoolContractAddress,
            EbridgeConst.tDVV => EbridgeConst.TokenPoolContractAddressTDVV,
            EbridgeConst.tDVW => EbridgeConst.TokenPoolContractAddressTDVW,
            _ => string.Empty
        };
    }

    protected async Task UpdateTokenPoolLiquidityAsync(string chainId, string symbol, long liquidity, string txId,
        string eventName, LiquidityType liquidityType, DateTime blockTime)
    {
        Logger.LogInformation(
            "UpdateTokenPoolLiquidityAsync start, chainId:{chainId}, symbol:{symbol}, liquidity:{liquidity}", chainId,
            symbol, liquidity);
        var id = IdGenerateHelper.GetId(chainId, txId, eventName);
        var tokenPool = new PoolLiquidityRecordIndex
        {
            Id = id,
            ChainId = chainId,
            TokenSymbol = symbol,
            Liquidity = liquidity,
            LiquidityType = liquidityType,
            UpdateTime = blockTime
        };
        await SaveEntityAsync(tokenPool);
    }
}