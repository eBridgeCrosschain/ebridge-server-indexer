using AElf;
using AElf.Contracts.MultiToken;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.CrossChain;

public class CrossChainReceivedProcessor: CrossChainProcessorBase<CrossChainReceived>
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
            ReceiveAmount = eventValue.Amount,
            ReceiveTime = context.BlockTime,
            FromChainId = ChainHelper.ConvertChainIdToBase58(eventValue.FromChainId),
            ReceiveTransactionId = context.TransactionId,
            ToChainId = context.ChainId,
            TransferTransactionId = eventValue.TransferTransactionId.ToHex(),
            ReceiveTokenSymbol = eventValue.Symbol,
            FromAddress = eventValue.From.ToBase58(),
            ToAddress = eventValue.To.ToBase58(),
            TransferType = TransferType.Receive,
            CrossChainType = CrossChainType.Homogeneous
        };
        ObjectMapper.Map<LogEventContext, CrossChainTransferInfoIndex>(context, info);

        await _repository.AddOrUpdateAsync(info);
    }
}