using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Oracle;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Oracle;

public class QueryCompletedWithAggregationProcessor : OracleProcessorBase<QueryCompletedWithAggregation>
{
    public override async Task ProcessAsync(QueryCompletedWithAggregation logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "QueryCompletedWithAggregationProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId,"QueryCompletedWithAggregation");
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.QueryCompleted
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}