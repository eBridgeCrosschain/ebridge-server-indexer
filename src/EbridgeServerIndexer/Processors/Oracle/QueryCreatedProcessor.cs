using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Oracle;

using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Oracle;

public class QueryCreatedProcessor: OracleProcessorBase<QueryCreated>
{
    public override async Task ProcessAsync(QueryCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "QueryCreatedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId,"QueryCreated");
        var receiptHash = logEvent.QueryInfo.Options[0].Split(".")[0];
        var starIndex = Convert.ToInt64(logEvent.QueryInfo.Options[0].Split(".")[1]);
        var endIndex = Convert.ToInt64(logEvent.QueryInfo.Options[1].Split(".")[1]);

        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            ReceiptHash = receiptHash,
            StartIndex = starIndex,
            EndIndex = endIndex,
            Step = OracleStep.QueryCreated,
            QueryId = logEvent.QueryId.ToHex(),
        };
        await SaveEntityAsync(info);
    }
}