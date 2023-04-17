using AElf.CSharp.Core;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.ObjectMapping;

namespace CrossChainServer.Indexer.Processors.Oracle;

public abstract class OracleProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly IObjectMapper ObjectMapper;
    protected readonly IAElfIndexerClientEntityRepository<OracleQueryInfoIndex, LogEventInfo> Repository;
    protected readonly ContractInfoOptions ContractInfoOptions;

    protected OracleProcessorBase(ILogger<OracleProcessorBase<TEvent>> logger, IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<OracleQueryInfoIndex, LogEventInfo> repository,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions)
        : base(logger)
    {
        ObjectMapper = objectMapper;
        Repository = repository;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].OracleContractAddress;
    }
    
    protected string GetOracleInfoId(LogEventContext context)
    {
        return IdGenerateHelper.GetId(context.ChainId, context.TransactionId, typeof(TEvent).Name);
    }
}