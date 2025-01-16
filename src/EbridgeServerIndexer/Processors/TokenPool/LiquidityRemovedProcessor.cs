using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.TokenPool;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.TokenPool;

public class LiquidityRemovedProcessor : TokenPoolProcessorBase<LiquidityRemoved>
{
    public override async Task ProcessAsync(LiquidityRemoved logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "LiquidityRemovedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId);
        var userLiquidity = new UserLiquidityRecordIndex
        {
            Id = id,
            ChainId = context.ChainId,
            Provider = logEvent.Provider.ToBase58(),
            TokenSymbol = logEvent.TokenSymbol,
            Liquidity = logEvent.Amount,
            LiquidityType = LiquidityType.Remove,
            UpdateTime = context.Block.BlockTime
        };
        
        await UpdateTokenPoolLiquidityAsync(context.ChainId, logEvent.TokenSymbol, logEvent.Amount,
            context.Transaction.TransactionId, "LiquidityRemoved", LiquidityType.Remove, context.Block.BlockTime);
        await SaveEntityAsync(userLiquidity);
    }
}