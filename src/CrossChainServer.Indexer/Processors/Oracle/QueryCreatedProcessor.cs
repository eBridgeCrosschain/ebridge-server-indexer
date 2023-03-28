using AElf.Contracts.Oracle;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Oracle;

public class QueryCreatedProcessor: OracleProcessorBase<QueryCreated>
{
    public QueryCreatedProcessor(ILogger<QueryCreatedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<OracleQueryInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger, objectMapper, repository, contractInfoOptions)
    {
    }

    protected override async Task HandleEventAsync(QueryCreated eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, context.TransactionId);
        var receiptHash = eventValue.QueryInfo.Options[0].Split(".")[0];
        var starIndex = Convert.ToInt64(eventValue.QueryInfo.Options[0].Split(".")[1]);
        var endIndex = Convert.ToInt64(eventValue.QueryInfo.Options[1].Split(".")[1]);

        var info = new OracleQueryInfoIndex()
        {
            Id = id,
            ReceiptHash = receiptHash,
            StartIndex = starIndex,
            EndIndex = endIndex,
            Step = OracleStep.QueryCreated,
            QueryId = eventValue.QueryId.ToHex(),
        };
        ObjectMapper.Map<LogEventContext, OracleQueryInfoIndex>(context, info);

        await Repository.AddOrUpdateAsync(info);
    }
}