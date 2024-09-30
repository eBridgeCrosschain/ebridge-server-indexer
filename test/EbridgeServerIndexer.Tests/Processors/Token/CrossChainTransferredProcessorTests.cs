// using AeFinder.Sdk;
// using EbridgeServerIndexer.Entities;
// using EbridgeServerIndexer.GraphQL;
// using Shouldly;
// using Volo.Abp.ObjectMapping;
// using AElf.Contracts.MultiToken;
// using AElf.Types;
// using Xunit;
//
// namespace EbridgeServerIndexer.Processors.Token;
//
// public class CrossChainTransferredProcessorTests : EbridgeServerIndexerTestBase
// {
//     private readonly CrossChainTransferredProcessor _crossChainTransferredProcessor;
//     private readonly IReadOnlyRepository<CrossChainTransferInfoIndex> _repository;
//     private readonly IObjectMapper _objectMapper;
//
//     public CrossChainTransferredProcessorTests()
//     {
//         _crossChainTransferredProcessor = GetRequiredService<CrossChainTransferredProcessor>();
//         _repository = GetRequiredService<IReadOnlyRepository<CrossChainTransferInfoIndex>>();
//         _objectMapper = GetRequiredService<IObjectMapper>();
//     }
//
//     [Fact]
//     public async Task Test()
//     {
//         var logEvent = new CrossChainTransferred
//         {
//            From = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
//               To = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
//                 Symbol = "ELF",
//                 Amount = 100,
//                 Memo = "memo",
//                 ToChainId = 1,
//                 IssueChainId = 2
//         };
//         var logEventContext = GenerateLogEventContext(logEvent);
//         await _crossChainTransferredProcessor.ProcessAsync(logEvent, logEventContext);
//         
//         var entities = await Query.CrossChainTransferInfo(_repository, _objectMapper, new QueryInput
//         {
//             ChainId = ChainId,
//             StartBlockHeight = 0,
//             EndBlockHeight = 100
//         });
//         entities.Count.ShouldBe(1);
//         entities[0].BlockHeight.ShouldBe(100); 
//         entities[0].FromAddress.ShouldBe(logEvent.From.ToBase58());
//         entities[0].CrossChainType.ShouldBe(CrossChainType.Homogeneous);
//         entities[0].BlockHeight.ShouldBe(100);
//         entities[0].FromChainId.ShouldBe(ChainId);
//         entities[0].TransferTransactionId.ShouldBe(logEventContext.Transaction.TransactionId);
//         entities[0].ToAddress.ShouldBe(logEvent.To.ToBase58());
//         entities[0].TransferAmount.ShouldBe(100);
//         entities[0].TransferTokenSymbol.ShouldBe("ELF");
//         entities[0].TransferType.ShouldBe(TransferType.Transfer);
//         entities[0].TransferTime.ShouldBe(logEventContext.Block.BlockTime);
//         entities[0].TransferBlockHeight.ShouldBe(logEventContext.Block.BlockHeight);
//         entities[0].ToChainId.ShouldBe("1");
//     }
// }