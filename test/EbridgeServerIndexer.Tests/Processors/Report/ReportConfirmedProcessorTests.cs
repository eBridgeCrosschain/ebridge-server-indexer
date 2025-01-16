using AeFinder.Sdk;
using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf;
using EBridge.Contracts.Oracle;
using EBridge.Contracts.Report;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;
using QueryInput = EbridgeServerIndexer.GraphQL.QueryInput;

namespace EbridgeServerIndexer.Processors.Report;

public class ReportConfirmedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly ReportConfirmedProcessor _reportConfirmedProcessor;
    private readonly IReadOnlyRepository<ReportInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public ReportConfirmedProcessorTests()
    {
        _reportConfirmedProcessor = GetRequiredService<ReportConfirmedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<ReportInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new ReportConfirmed
        {
            RoundId = 1,
            Signature = "signature",
            IsAllNodeConfirmed = true,
            RegimentId = HashHelper.ComputeFrom("regiment_id"),
            TargetChainId = "tDVW",
            Token = "token"
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _reportConfirmedProcessor.ProcessAsync(logEvent, logEventContext);

        var entities = await Query.ReportInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 5,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(1);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].RoundId.ShouldBe(logEvent.RoundId);
        entities[0].Token.ShouldBe(logEvent.Token);
        entities[0].Step.ShouldBe(ReportStep.Confirmed);
        entities[0].TargetChainId.ShouldBe("tDVW");
    }
}