using AElf.Contracts.Oracle;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Oracle;

public class CommittedProcessor : OracleProcessorBase<Committed>
{
    public CommittedProcessor(ILogger<CommittedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<OracleQueryInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger, objectMapper, repository, contractInfoOptions)
    {
    }

    protected override async Task HandleEventAsync(Committed eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, context.TransactionId);
        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            Step = OracleStep.Committed,
            QueryId = eventValue.QueryId.ToHex(),
        };
        ObjectMapper.Map<LogEventContext, OracleQueryInfoIndex>(context, info);

        await Repository.AddOrUpdateAsync(info);
    }
}