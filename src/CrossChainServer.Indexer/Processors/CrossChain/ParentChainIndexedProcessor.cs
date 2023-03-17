using System.Threading.Tasks;
using AElf;
using AElf.Contracts.CrossChain;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.CrossChain;

public class ParentChainIndexedProcessor: CrossChainProcessorBase<ParentChainIndexed>
{
    private readonly IAElfIndexerClientEntityRepository<CrossChainIndexingInfoIndex, LogEventInfo> _repository;

    public ParentChainIndexedProcessor(ILogger<ParentChainIndexedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<CrossChainIndexingInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger,objectMapper, contractInfoOptions)
    {
        _repository = repository;
    }
    
    protected override async Task HandleEventAsync(ParentChainIndexed eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, context.TransactionId, eventValue.ChainId);

        var info = new CrossChainIndexingInfoIndex
        {
            Id = id,
            BlockTime = context.BlockTime,
            IndexChainId = ChainHelper.ConvertChainIdToBase58(eventValue.ChainId),
            IndexBlockHeight = eventValue.IndexedHeight
        };
        ObjectMapper.Map<LogEventContext, CrossChainIndexingInfoIndex>(context, info);

        await _repository.AddOrUpdateAsync(info);
    }
}