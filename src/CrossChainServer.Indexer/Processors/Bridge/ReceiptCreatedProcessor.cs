using AElf.Contracts.Bridge;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Bridge;

public class ReceiptCreatedProcessor : BridgeProcessorBase<ReceiptCreated>
{
    private readonly IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> _repository;
    
    public ReceiptCreatedProcessor(ILogger<ReceiptCreatedProcessor> logger, IObjectMapper objectMapper,
            IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> repository,
            IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger,objectMapper, contractInfoOptions, repository)
    {
        _repository = repository;
    }
    
    protected override async Task HandleEventAsync(ReceiptCreated eventValue, LogEventContext context)
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
            CrossChainType = CrossChainType.Heterogeneous
        };
        ObjectMapper.Map(context, info);
        ObjectMapper.Map(eventValue, info);

        await _repository.AddOrUpdateAsync(info);
    }
}