using AeFinder.Sdk;
using AElf;
using AElf.Types;
using EBridge.Contracts.Oracle;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;
using QueryInput = EbridgeServerIndexer.GraphQL.QueryInput;

namespace EbridgeServerIndexer.Processors.Oracle;

public class CommittedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly CommittedProcessor _committedProcessor;
    private readonly IReadOnlyRepository<OracleQueryInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public CommittedProcessorTests()
    {
        _committedProcessor = GetRequiredService<CommittedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<OracleQueryInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new Committed
        {
            QueryId = HashHelper.ComputeFrom("queryid"),
            OracleNodeAddress = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            Commitment = HashHelper.ComputeFrom("commitment")
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _committedProcessor.ProcessAsync(logEvent, logEventContext);

        var entities = await Query.OracleQueryInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 0,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(1);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].QueryId.ShouldBe(logEvent.QueryId.ToHex());
    }
}