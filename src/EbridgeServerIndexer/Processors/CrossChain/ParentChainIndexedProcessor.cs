using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.CrossChain;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.CrossChain;

public class ParentChainIndexedProcessor : CrossChainProcessorBase<ParentChainIndexed>
{
    public override async Task ProcessAsync(ParentChainIndexed logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "ParentChainIndexedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId, logEvent.ChainId);

        var info = new CrossChainIndexingInfoIndex
        {
            Id = id
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}