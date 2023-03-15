using System.Threading.Tasks;
using AElf.Contracts.Report;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Report;

public class ReportProposedProcessor: ReportProcessorBase<ReportProposed>
{
    public ReportProposedProcessor(ILogger<ReportProposedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<ReportInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger, objectMapper, repository, contractInfoOptions)
    {
    }
    
    protected override async Task HandleEventAsync(ReportProposed eventValue, LogEventContext context)
    {
        if (!eventValue.QueryInfo.Title.StartsWith("lock_token_"))
        {
            return;
        }

        var reportId = GetReportInfoId(context.ChainId, eventValue.RoundId, eventValue.Token,
            eventValue.TargetChainId);

        var reportInfo = new ReportInfoIndex()
        {
            Id = reportId,
            ReceiptId = eventValue.QueryInfo.Title.Split("_")[2],
            ReceiptHash = eventValue.QueryInfo.Options[0],
            RoundId = eventValue.RoundId,
            Token = eventValue.Token,
            TargetChainId = eventValue.TargetChainId,
            Step = ReportStep.Proposed
        };
        ObjectMapper.Map<LogEventContext, ReportInfoIndex>(context, reportInfo);
        await Repository.AddOrUpdateAsync(reportInfo);
    }
}