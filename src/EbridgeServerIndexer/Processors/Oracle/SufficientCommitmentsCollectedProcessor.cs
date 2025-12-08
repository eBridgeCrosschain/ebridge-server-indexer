using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using EBridge.Contracts.Oracle;

using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.Processors.Oracle;

public class SufficientCommitmentsCollectedProcessor : OracleProcessorBase<SufficientCommitmentsCollected>
{
    public override async Task ProcessAsync(SufficientCommitmentsCollected logEvent, LogEventContext context)
    {
        Logger.LogInformation(
            "SufficientCommitmentsCollectedProcessor start, blockHeight:{Height}, blockHash:{Hash}, txId:{txId}",
            context.Block.BlockHeight,
            context.Block.BlockHash,
            context.Transaction.TransactionId);
        var id = IdGenerateHelper.GetId(context.ChainId, context.Transaction.TransactionId,"SufficientCommitmentsCollected");
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.SufficientCommitmentsCollected
        };
        ObjectMapper.Map(logEvent, info);
        await SaveEntityAsync(info);
    }
}