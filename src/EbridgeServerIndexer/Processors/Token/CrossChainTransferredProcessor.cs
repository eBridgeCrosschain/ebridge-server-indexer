using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Token;

public class CrossChainTransferredProcessor: TokenProcessorBase<CrossChainTransferred>
{
    public override async Task ProcessAsync(CrossChainTransferred logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "CrossChainTransferredProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
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
            CrossChainType = CrossChainType.Homogeneous
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}