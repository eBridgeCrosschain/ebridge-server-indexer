using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Token;

public class CrossChainReceivedProcessor: TokenProcessorBase<CrossChainReceived>
{
    public override async Task ProcessAsync(CrossChainReceived logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "CrossChainReceivedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
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
            CrossChainType = CrossChainType.Homogeneous
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}