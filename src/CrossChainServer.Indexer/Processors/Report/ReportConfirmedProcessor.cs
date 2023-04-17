using AElf.Contracts.Report;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Report;

public class ReportConfirmedProcessor: ReportProcessorBase<ReportConfirmed>
{
    public ReportConfirmedProcessor(ILogger<ReportConfirmedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<ReportInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger, objectMapper, repository, contractInfoOptions)
    {
    }

    protected override async Task HandleEventAsync(ReportConfirmed eventValue, LogEventContext context)
    {
        if (!eventValue.IsAllNodeConfirmed)
        {
            return;
        }
        var id = GetReportInfoId(context);
        var reportInfo = new ReportInfoIndex()
        {
            Id = id,
            Step = ReportStep.Confirmed
        };
        ObjectMapper.Map(context, reportInfo);
        ObjectMapper.Map(eventValue, reportInfo);

        await Repository.AddOrUpdateAsync(reportInfo);
    }
}