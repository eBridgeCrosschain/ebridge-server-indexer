using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Oracle;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Oracle;

public class CommittedProcessor : OracleProcessorBase<Committed>
{
    public override async Task ProcessAsync(Committed logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "CommittedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId,"Committed");
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.Committed
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}