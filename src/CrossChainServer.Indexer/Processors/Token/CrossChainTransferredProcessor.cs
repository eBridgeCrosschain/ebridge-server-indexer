using AElf;
using AElf.Contracts.MultiToken;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Token;

public class CrossChainTransferredProcessor: TokenProcessorBase<CrossChainTransferred>
{
    private readonly IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> _repository;

    public CrossChainTransferredProcessor(ILogger<CrossChainTransferredProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger,objectMapper, contractInfoOptions)
    {
        _repository = repository;
    }
    
    protected override async Task HandleEventAsync(CrossChainTransferred eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, context.TransactionId);

        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            FromChainId = context.ChainId,
            TransferBlockHeight = context.BlockHeight,
            TransferTime = context.BlockTime,
            TransferTransactionId = context.TransactionId,
            TransferType = TransferType.Transfer,
            CrossChainType = CrossChainType.Homogeneous
        };
        ObjectMapper.Map(context, info);
        ObjectMapper.Map(eventValue, info);

        await _repository.AddOrUpdateAsync(info);
    }
}