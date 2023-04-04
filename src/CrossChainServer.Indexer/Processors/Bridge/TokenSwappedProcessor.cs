using AElf.Contracts.Bridge;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using CrossChainServer.Indexer.Processors.CrossChain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Bridge;

public class TokenSwappedProcessor: BridgeProcessorBase<TokenSwapped>
{
    private readonly IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> _repository;

    public TokenSwappedProcessor(ILogger<TokenSwappedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger,objectMapper, contractInfoOptions,repository)
    {
        _repository = repository;
    }
    
    protected override async Task HandleEventAsync(TokenSwapped eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, context.TransactionId);

        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            ReceiveTime = context.BlockTime,
            ReceiveTransactionId = context.TransactionId,
            ToChainId = context.ChainId,
            TransferType = TransferType.Receive,
            CrossChainType = CrossChainType.Heterogeneous
        };
        ObjectMapper.Map(context, info);
        ObjectMapper.Map(eventValue, info);

        await _repository.AddOrUpdateAsync(info);
    }
}