// using AeFinder.Sdk;
// using EbridgeServerIndexer.Entities;
// using EbridgeServerIndexer.GraphQL;
// using Shouldly;
// using Volo.Abp.ObjectMapping;
// using Xunit;
// using CrossChainReceived = AElf.Contracts.MultiToken.CrossChainReceived;
//
// namespace EbridgeServerIndexer.Processors.Token;
//
// public class CrossChainReceivedProcessorTests : EbridgeServerIndexerTestBase
// {
//     private readonly CrossChainReceivedProcessor _crossChainReceivedProcessor;
//     private readonly IReadOnlyRepository<CrossChainTransferInfoIndex> _repository;
//     private readonly IObjectMapper _objectMapper;
//
//     public CrossChainReceivedProcessorTests()
//     {
//         _crossChainReceivedProcessor = GetRequiredService<CrossChainReceivedProcessor>();
//         _repository = GetRequiredService<IReadOnlyRepository<CrossChainTransferInfoIndex>>();
//         _objectMapper = GetRequiredService<IObjectMapper>();
//     }
//
//     [Fact]
//     public async Task Test()
//     {
//         /*
//          * From = From,
//         To = To,
//         Symbol = Symbol,
//         Amount = Amount,
//         Memo = Memo,
//         ToChainId = ToChainId,
//         IssueChainId = IssueChainId,
//          */
//         var logEvent = new CrossChainReceived
//         {
//            
//         };
//         var logEventContext = GenerateLogEventContext(logEvent);
//         await _crossChainReceivedProcessor.ProcessAsync(logEvent, logEventContext);
//         
//         var entities = await Query.CrossChainTransferInfo(_repository, _objectMapper, new QueryInput
//         {
//             ChainId = ChainId,
//             StartBlockHeight = 0,
//             EndBlockHeight = 100
//         });
//         entities.Count.ShouldBe(1);
//         entities[0].BlockHeight.ShouldBe(100); 
//        
//     }
// }