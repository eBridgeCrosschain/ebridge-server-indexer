using AeFinder.Sdk;
using AElf.Types;
using EBridge.Contracts.Bridge;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EbridgeServerIndexer.Processors.Bridge;

public class TokenSwappedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly TokenSwappedProcessor _tokenSwappedProcessor;
    private readonly IReadOnlyRepository<CrossChainTransferInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public TokenSwappedProcessorTests()
    {
        _tokenSwappedProcessor = GetRequiredService<TokenSwappedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<CrossChainTransferInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new TokenSwapped
        {
            ReceiptId = "ReceiptId",
            Amount = 100,
            Symbol = "ELF", 
            Address = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            FromChainId = "AELF"
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _tokenSwappedProcessor.ProcessAsync(logEvent, logEventContext);
        
        var logEvent1 = new TokenSwapped
        {
            ReceiptId = "ReceiptIdA",
            Amount = 300,
            Symbol = "USDT", 
            Address = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            FromChainId = "tDVW"
        };
        var logEventContext1 = GenerateLogEventContext(logEvent1);
        logEventContext1.Transaction.TransactionId = "3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02";
        await _tokenSwappedProcessor.ProcessAsync(logEvent1, logEventContext1);

        var entities = await Query.CrossChainTransferInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 0,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(2);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].FromChainId.ShouldBe("AELF");
        entities[0].ReceiveTransactionId.ShouldBe(logEventContext.Transaction.TransactionId);
        entities[0].ToAddress.ShouldBe("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C");
        entities[0].ReceiptId.ShouldBe("ReceiptId");
        entities[0].ReceiveAmount.ShouldBe(100);
        entities[0].ReceiveTokenSymbol.ShouldBe("ELF");
        entities[0].TransferType.ShouldBe(TransferType.Receive);
        entities[0].CrossChainType.ShouldBe(CrossChainType.Heterogeneous);
        entities[0].ReceiveTime.ShouldBe(logEventContext.Block.BlockTime);
        entities[0].ToChainId.ShouldBe("AELF");
        
        entities[1].BlockHeight.ShouldBe(100);
        entities[1].FromChainId.ShouldBe("tDVW");
        entities[1].ReceiveTransactionId.ShouldBe(logEventContext1.Transaction.TransactionId);
        entities[1].ToAddress.ShouldBe("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C");
        entities[1].ReceiptId.ShouldBe("ReceiptIdA");
        entities[1].ReceiveAmount.ShouldBe(300);
        entities[1].ReceiveTokenSymbol.ShouldBe("USDT");
        entities[1].TransferType.ShouldBe(TransferType.Receive);
        entities[1].CrossChainType.ShouldBe(CrossChainType.Heterogeneous);
        entities[1].ReceiveTime.ShouldBe(logEventContext1.Block.BlockTime);
        entities[1].ToChainId.ShouldBe(ChainId);
    }
}