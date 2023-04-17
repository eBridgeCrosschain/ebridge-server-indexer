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

public class CrossChainReceivedProcessor: TokenProcessorBase<CrossChainReceived>
{
    private readonly IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> _repository;

    public CrossChainReceivedProcessor(ILogger<CrossChainReceivedProcessor> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger,objectMapper, contractInfoOptions)
    {
        _repository = repository;
    }

    protected override async Task HandleEventAsync(CrossChainReceived eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, context.TransactionId);

        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            ReceiveTime = context.BlockTime,
            ReceiveTransactionId = context.TransactionId,
            ToChainId = context.ChainId,
            TransferType = TransferType.Receive,
            CrossChainType = CrossChainType.Homogeneous
        };
        ObjectMapper.Map(context, info);
        ObjectMapper.Map(eventValue, info);

        await _repository.AddOrUpdateAsync(info);
    }
}