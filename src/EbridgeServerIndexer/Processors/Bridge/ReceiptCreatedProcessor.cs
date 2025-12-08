using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Bridge;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Bridge;

public class ReceiptCreatedProcessor : BridgeProcessorBase<ReceiptCreated>
{
    public override async Task ProcessAsync(ReceiptCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "ReceiptCreatedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}", context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId);
        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            FromChainId = context.ChainId,
            TransferBlockHeight = context.Block.BlockHeight,
            TransferTime = context.Block.BlockTime,
            TransferTransactionId = context.Transaction.TransactionId,
            TransferType = TransferType.Transfer,
            CrossChainType = CrossChainType.Heterogeneous
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}