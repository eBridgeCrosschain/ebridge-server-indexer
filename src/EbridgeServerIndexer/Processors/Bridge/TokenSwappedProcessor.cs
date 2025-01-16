using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Bridge;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Bridge;

public class TokenSwappedProcessor : BridgeProcessorBase<TokenSwapped>
{
    public override async Task ProcessAsync(TokenSwapped logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "TokenSwappedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId);

        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            ReceiveTime = context.Block.BlockTime,
            ReceiveTransactionId = context.Transaction.TransactionId,
            ToChainId = context.ChainId,
            TransferType = TransferType.Receive,
            CrossChainType = CrossChainType.Heterogeneous
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}