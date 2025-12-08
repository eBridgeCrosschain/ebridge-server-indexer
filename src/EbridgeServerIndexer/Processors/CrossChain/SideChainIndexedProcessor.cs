using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.CrossChain;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.CrossChain;

public class SideChainIndexedProcessor : CrossChainProcessorBase<SideChainIndexed>
{
    public override async Task ProcessAsync(SideChainIndexed logEvent, LogEventContext context)
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