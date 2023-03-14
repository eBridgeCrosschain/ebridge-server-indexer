using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using CrossChainServer.Indexer.Processors.CrossChain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Bridge;

public class BridgeProcessorBase<TEvent>: AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> Repository;

    protected BridgeProcessorBase(ILogger<CrossChainProcessorBase<TEvent>> logger, IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<CrossChainTransferInfoIndex, LogEventInfo> repository)
        : base(logger)
    {
        ObjectMapper = objectMapper;
        Repository = repository;
        ContractInfoOptions = contractInfoOptions.Value;
    }
    
    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].BridgeContractAddress;
    }
}