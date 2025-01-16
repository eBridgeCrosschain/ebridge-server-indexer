using AeFinder.Sdk;
using EBridge.Contracts.TokenPool;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EbridgeServerIndexer.Processors.TokenPool;

public class LockedProcessorTests: EbridgeServerIndexerTestBase
{
    private readonly LockedProcessor _lockedProcessor;
    private readonly IReadOnlyRepository<PoolLiquidityRecordIndex> _poolRepository;

    private readonly IObjectMapper _objectMapper;

    public LockedProcessorTests()
    {
        _lockedProcessor = GetRequiredService<LockedProcessor>();
        _poolRepository = GetRequiredService<IReadOnlyRepository<PoolLiquidityRecordIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }
    [Fact]
    public async Task Test()
    {
        var logEvent = new Locked
        {
            TargetTokenSymbol = "ELF",
            Amount = 100
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _lockedProcessor.ProcessAsync(logEvent, logEventContext);
        
        var logEvent1 = new Locked
        {
            TargetTokenSymbol = "ELF",
            Amount = 50
        };
        var logEventContext1 = GenerateLogEventContext(logEvent1);
        logEventContext1.Transaction.TransactionId = "3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02";
        await _lockedProcessor.ProcessAsync(logEvent1, logEventContext1);

        var entities1 = await Query.PoolLiquidityInfo(_poolRepository, _objectMapper, new QueryInput
        {
            ChainId = ChainId
        });
        entities1.Count.ShouldBe(2);
        entities1[0].BlockHeight.ShouldBe(100);
        entities1[0].ChainId.ShouldBe(ChainId);
        entities1[0].Liquidity.ShouldBe(100);
        entities1[0].TokenSymbol.ShouldBe("ELF");
        entities1[0].LiquidityType.ShouldBe(LiquidityType.Add);

        entities1[1].ChainId.ShouldBe(ChainId);
        entities1[1].Liquidity.ShouldBe(50);
        entities1[1].TokenSymbol.ShouldBe("ELF");
        entities1[1].LiquidityType.ShouldBe(LiquidityType.Add);
    }
}