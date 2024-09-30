using AeFinder.Sdk;
using AElf.Contracts.CrossChain;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EbridgeServerIndexer.Processors.CrossChain;

public class ParentChainIndexedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly ParentChainIndexedProcessor _parentChainIndexedProcessor;
    private readonly IReadOnlyRepository<CrossChainIndexingInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public ParentChainIndexedProcessorTests()
    {
        _parentChainIndexedProcessor = GetRequiredService<ParentChainIndexedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<CrossChainIndexingInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new ParentChainIndexed
        {
            ChainId = 1,
            IndexedHeight = 100
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _parentChainIndexedProcessor.ProcessAsync(logEvent, logEventContext);

        var entities = await Query.CrossChainIndexingInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 0,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(1);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].ChainId.ShouldBe(ChainId);
        entities[0].IndexBlockHeight.ShouldBe(100);
        entities[0].IndexChainId.ShouldBe("LUw");
        entities[0].BlockHash.ShouldBe(logEventContext.Block.BlockHash);
    }
}