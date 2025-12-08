using AeFinder.Sdk;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;
using AElf.Contracts.MultiToken;
using AElf.Types;

namespace EbridgeServerIndexer.Processors.Token;

public class CrossChainReceivedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly CrossChainReceivedProcessor _crossChainReceivedProcessor;
    private readonly IReadOnlyRepository<CrossChainTransferInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public CrossChainReceivedProcessorTests()
    {
        _crossChainReceivedProcessor = GetRequiredService<CrossChainReceivedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<CrossChainTransferInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new CrossChainReceived
        {
            From = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            To = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            Symbol = "ELF",
            Amount = 100,
            Memo = "Memo",
            FromChainId = 1,
            IssueChainId = 1,
            ParentChainHeight = 100,
            TransferTransactionId = Hash.LoadFromHex("3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02"),
        };
        
        var logEventContext = GenerateLogEventContext(logEvent);
        await _crossChainReceivedProcessor.ProcessAsync(logEvent, logEventContext);
        
        var entities = await Query.CrossChainTransferInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 5,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(1);
        entities[0].BlockHeight.ShouldBe(100); 
        
                
        var entity = await Query.HomogeneousCrossChainTransferInfo(_repository, _objectMapper, new GetCrossChainInfoInput
        {
            ChainId = ChainId,
            TransactionId = "3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02"
        });
        entity.ShouldNotBeNull();
        entity.BlockHeight.ShouldBe(100);
        entity.FromChainId.ShouldBe("LUw");
        entity.TransferTransactionId.ShouldBe("3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02");
        entity.ReceiveTransactionId.ShouldBe(logEventContext.Transaction.TransactionId);
       
    }
}