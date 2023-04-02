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
            TransferAmount = eventValue.Amount,
            FromAddress = eventValue.Owner.ToBase58(),
            ToAddress = eventValue.TargetAddress,
            TransferTokenSymbol = eventValue.Symbol,
            FromChainId = context.ChainId,
            ToChainId = eventValue.TargetChainId,
            TransferBlockHeight = context.BlockHeight,
            TransferTime = context.BlockTime,
            TransferTransactionId = context.TransactionId,
            TransferType = TransferType.Transfer,
            CrossChainType = CrossChainType.Heterogeneous,
            ReceiptId = eventValue.ReceiptId
        };
        ObjectMapper.Map<LogEventContext, CrossChainTransferInfoIndex>(context, info);

        await _repository.AddOrUpdateAsync(info);
    }
}