using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Oracle;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Oracle;

public class QueryCompletedWithoutAggregationProcessor : OracleProcessorBase<QueryCompletedWithoutAggregation>
{
    public override async Task ProcessAsync(QueryCompletedWithoutAggregation logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "QueryCompletedWithoutAggregationProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId,"QueryCompletedWithoutAggregation");
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.QueryCompleted
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}