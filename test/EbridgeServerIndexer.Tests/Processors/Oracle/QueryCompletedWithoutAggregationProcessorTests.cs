using AeFinder.Sdk;
using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf;
using EBridge.Contracts.Oracle;

using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;
using QueryInput = EbridgeServerIndexer.GraphQL.QueryInput;

namespace EbridgeServerIndexer.Processors.Oracle;

public class QueryCompletedWithoutAggregationProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly QueryCompletedWithoutAggregationProcessor _queryCompletedWithoutAggregationProcessor;
    private readonly IReadOnlyRepository<OracleQueryInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public QueryCompletedWithoutAggregationProcessorTests()
    {
        _queryCompletedWithoutAggregationProcessor = GetRequiredService<QueryCompletedWithoutAggregationProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<OracleQueryInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new QueryCompletedWithoutAggregation
        {
            QueryId = HashHelper.ComputeFrom("queryid"),
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _queryCompletedWithoutAggregationProcessor.ProcessAsync(logEvent, logEventContext);

        var entities = await Query.OracleQueryInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 5,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(1);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].QueryId.ShouldBe(logEvent.QueryId.ToHex());
        entities[0].Step.ShouldBe(OracleStep.QueryCompleted);
    }
}