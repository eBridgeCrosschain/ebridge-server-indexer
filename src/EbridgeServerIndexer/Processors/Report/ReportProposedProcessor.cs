using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Report;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Report;

public class ReportProposedProcessor: ReportProcessorBase<ReportProposed>
{
    public override async Task ProcessAsync(ReportProposed logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "ReportProposedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        if (!logEvent.QueryInfo.Title.StartsWith("lock_token_"))
        {
            return;
        }

        var id= IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId, "ReportProposed");
        var receiptInfo = logEvent.QueryInfo.Options.Count > 1 ? logEvent.QueryInfo.Options[1] : null;
        var reportInfo = new ReportInfoIndex()
        {
            Id = id,
            ReceiptId = logEvent.QueryInfo.Title.Split("_")[2],
            Step = ReportStep.Proposed,
            ReceiptInfo = receiptInfo
        };
        ObjectMapper.Map(logEvent, reportInfo);
        await SaveEntityAsync(reportInfo);
    }
}