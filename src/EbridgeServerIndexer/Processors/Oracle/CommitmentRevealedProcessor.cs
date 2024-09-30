using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Oracle;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Oracle;

public class CommitmentRevealedProcessor : OracleProcessorBase<CommitmentRevealed>
{
    public override async Task ProcessAsync(CommitmentRevealed logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "CommitmentRevealedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId,"CommitmentRevealed");
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.CommitmentRevealed
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}