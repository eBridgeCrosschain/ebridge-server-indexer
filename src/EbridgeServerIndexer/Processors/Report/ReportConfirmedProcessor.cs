using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Report;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Report;

public class ReportConfirmedProcessor : ReportProcessorBase<ReportConfirmed>
{
    public override async Task ProcessAsync(ReportConfirmed logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "ReportConfirmedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        if (!logEvent.IsAllNodeConfirmed)
        {
            return;
        }

        var id= IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId, "ReportConfirmed");
        var reportInfo = new ReportInfoIndex()
        {
            Id = id,
            Step = ReportStep.Confirmed
        };
        ObjectMapper.Map(logEvent, reportInfo);
        await SaveEntityAsync(reportInfo);
    }
}