using AElf.Contracts.Oracle;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Oracle;

public class QueryCompletedWithoutAggregationProcessor : OracleProcessorBase<QueryCompletedWithoutAggregation>
{
    public QueryCompletedWithoutAggregationProcessor(ILogger<QueryCompletedWithoutAggregationProcessor> logger,
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<OracleQueryInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger, objectMapper, repository, contractInfoOptions)
    {
    }

    protected override async Task HandleEventAsync(QueryCompletedWithoutAggregation eventValue, LogEventContext context)
    {
        var id = GetOracleInfoId(context);
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.QueryCompleted
        };
        ObjectMapper.Map(context, info);
        ObjectMapper.Map(eventValue, info);

        await Repository.AddOrUpdateAsync(info);
    }
}