using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.TokenPool; 
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.TokenPool;

public class ReleasedProcessor: TokenPoolProcessorBase<Released>
{
    public override async Task ProcessAsync(Released logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "ReleasedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        await UpdateTokenPoolLiquidityAsync(context.ChainId, logEvent.TargetTokenSymbol, logEvent.Amount,
            context.Transaction.TransactionId, "Released", LiquidityType.Remove, context.Block.BlockTime);
    }
}