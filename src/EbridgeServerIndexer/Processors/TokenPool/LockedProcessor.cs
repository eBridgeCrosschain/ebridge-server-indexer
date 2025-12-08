using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.TokenPool;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.TokenPool;

public class LockedProcessor: TokenPoolProcessorBase<Locked>
{
    public override async Task ProcessAsync(Locked logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "LockedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        await UpdateTokenPoolLiquidityAsync(context.ChainId, logEvent.TargetTokenSymbol, logEvent.Amount,
            context.Transaction.TransactionId, "Locked", LiquidityType.Add, context.Block.BlockTime);
    }
}