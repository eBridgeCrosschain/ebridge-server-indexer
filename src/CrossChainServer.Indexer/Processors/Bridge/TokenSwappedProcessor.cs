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

public class TokenSwappedProcessor: CrossChainProcessorBase<TokenSwapped>
{
    private readonly IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> _repository;

    public TokenSwappedProcessor(ILogger<TokenSwappedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger,objectMapper, contractInfoOptions)
    {
        _repository = repository;
    }
    
    protected override async Task HandleEventAsync(TokenSwapped eventValue, LogEventContext context)
    {
        var id = context.TransactionId;

        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            ReceiveAmount = eventValue.Amount,
            ReceiveTime = context.BlockTime,
            FromChainId = eventValue.FromChainId,
            ReceiveTransactionId = context.TransactionId,
            ToChainId = context.ChainId,
            ReceiveTokenSymbol = eventValue.Symbol,
            ToAddress = eventValue.Address.ToBase58(),
            ReceiptId = eventValue.ReceiptId,
            TransferType = TransferType.Receive
        };
        ObjectMapper.Map<LogEventContext, CrossChainTransferInfoIndex>(context, info);

        await _repository.AddOrUpdateAsync(info);
    }
}