using AeFinder.Sdk;
using AElf.Types;
using EBridge.Contracts.TokenPool;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EbridgeServerIndexer.Processors.TokenPool;

public class LiquidityRemovedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly LiquidityRemovedProcessor _liquidityRemovedProcessor;
    private readonly IReadOnlyRepository<UserLiquidityRecordIndex> _repository;
    private readonly IReadOnlyRepository<PoolLiquidityRecordIndex> _poolRepository;

    private readonly IObjectMapper _objectMapper;

    public LiquidityRemovedProcessorTests()
    {
        _liquidityRemovedProcessor = GetRequiredService<LiquidityRemovedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<UserLiquidityRecordIndex>>();
        _poolRepository = GetRequiredService<IReadOnlyRepository<PoolLiquidityRecordIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }
    [Fact]
    public async Task Test()
    {
        var logEvent = new LiquidityRemoved
        {
            Provider = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            TokenSymbol = "ELF",
            Amount = 100
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _liquidityRemovedProcessor.ProcessAsync(logEvent, logEventContext);
        
        var logEvent1 = new LiquidityRemoved
        {
            Provider = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            TokenSymbol = "ELF",
            Amount = 50
        };
        var logEventContext1 = GenerateLogEventContext(logEvent1);
        logEventContext1.Transaction.TransactionId = "3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02";
        await _liquidityRemovedProcessor.ProcessAsync(logEvent1, logEventContext1);

        var entities = await Query.UserLiquidityInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId
        });
        entities.Count.ShouldBe(2);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].ChainId.ShouldBe(ChainId);
        entities[0].Provider.ShouldBe("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C");
        entities[0].Liquidity.ShouldBe(100);
        entities[0].TokenSymbol.ShouldBe("ELF");
        entities[0].LiquidityType.ShouldBe(LiquidityType.Remove);

        entities[1].ChainId.ShouldBe(ChainId);
        entities[1].Provider.ShouldBe("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C");
        entities[1].Liquidity.ShouldBe(50);
        entities[1].TokenSymbol.ShouldBe("ELF");
        entities[1].LiquidityType.ShouldBe(LiquidityType.Remove);
        
        var entities1 = await Query.PoolLiquidityInfo(_poolRepository, _objectMapper, new QueryInput
        {
            ChainId = ChainId
        });
        entities1.Count.ShouldBe(2);
        entities1[0].BlockHeight.ShouldBe(100);
        entities1[0].ChainId.ShouldBe(ChainId);
        entities1[0].Liquidity.ShouldBe(100);
        entities1[0].TokenSymbol.ShouldBe("ELF");
        entities1[0].LiquidityType.ShouldBe(LiquidityType.Remove);

        entities1[1].ChainId.ShouldBe(ChainId);
        entities1[1].Liquidity.ShouldBe(50);
        entities1[1].TokenSymbol.ShouldBe("ELF");
        entities1[1].LiquidityType.ShouldBe(LiquidityType.Remove);
    }
}