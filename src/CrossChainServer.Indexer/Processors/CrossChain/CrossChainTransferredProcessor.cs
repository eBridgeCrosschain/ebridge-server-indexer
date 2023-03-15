using System.Threading.Tasks;
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

public class CrossChainTransferredProcessor: CrossChainProcessorBase<CrossChainTransferred>
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
        var id = context.TransactionId;

        var info = new CrossChainTransferInfoIndex
        {
            Id = id,
            TransferAmount = eventValue.Amount,
            FromAddress = eventValue.From.ToBase58(),
            ToAddress = eventValue.To.ToBase58(),
            TransferTokenSymbol = eventValue.Symbol,
            FromChainId = context.ChainId,
            ToChainId = ChainHelper.ConvertChainIdToBase58(eventValue.ToChainId),
            TransferBlockHeight = context.BlockHeight,
            TransferTime = context.BlockTime,
            TransferTransactionId = context.TransactionId,
            TransferType = TransferType.Transfer
        };
        ObjectMapper.Map<LogEventContext, CrossChainTransferInfoIndex>(context, info);

        await _repository.AddOrUpdateAsync(info);
    }
}