using AeFinder.Sdk;
using AElf.Types;
using EBridge.Contracts.Bridge;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace EbridgeServerIndexer.Processors.Bridge;

public class ReceiptCreatedProcessorTests : EbridgeServerIndexerTestBase
{
    private readonly ReceiptCreatedProcessor _receiptCreatedProcessor;
    private readonly IReadOnlyRepository<CrossChainTransferInfoIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public ReceiptCreatedProcessorTests()
    {
        _receiptCreatedProcessor = GetRequiredService<ReceiptCreatedProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<CrossChainTransferInfoIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task Test()
    {
        var logEvent = new ReceiptCreated
        {
            TargetAddress = "TargetAddress",
            ReceiptId = "ReceiptId",
            Amount = 100,
            Owner = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            Symbol = "ELF",
            TargetChainId = "TargetChainId"
        };
        var logEventContext = GenerateLogEventContext(logEvent);
        await _receiptCreatedProcessor.ProcessAsync(logEvent, logEventContext);
        
        var logEvent1 = new ReceiptCreated
        {
            TargetAddress = "TargetAddressA",
            ReceiptId = "ReceiptIdA",
            Amount = 200,
            Owner = Address.FromBase58("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C"),
            Symbol = "USDT",
            TargetChainId = "tDVW"
        };
        var logEventContext1 = GenerateLogEventContext(logEvent1);
        logEventContext1.Transaction.TransactionId = "3ed1b52416b96aa061f4582b343908ed44f04842eb6b79b8376b6f300b70ce02";
        await _receiptCreatedProcessor.ProcessAsync(logEvent1, logEventContext1);

        var entities = await Query.CrossChainTransferInfo(_repository, _objectMapper, new QueryInput
        {
            ChainId = ChainId,
            StartBlockHeight = 5,
            EndBlockHeight = 100
        });
        entities.Count.ShouldBe(2);
        entities[0].BlockHeight.ShouldBe(100);
        entities[0].FromChainId.ShouldBe(ChainId);
        entities[0].TransferTransactionId.ShouldBe(logEventContext.Transaction.TransactionId);
        entities[0].FromAddress.ShouldBe("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C");
        entities[0].ToAddress.ShouldBe("TargetAddress");
        entities[0].ReceiptId.ShouldBe("ReceiptId");
        entities[0].TransferAmount.ShouldBe(100);
        entities[0].TransferTokenSymbol.ShouldBe("ELF");
        entities[0].TransferType.ShouldBe(TransferType.Transfer);
        entities[0].CrossChainType.ShouldBe(CrossChainType.Heterogeneous);
        entities[0].TransferTime.ShouldBe(logEventContext.Block.BlockTime);
        entities[0].TransferBlockHeight.ShouldBe(logEventContext.Block.BlockHeight);
        entities[0].ToChainId.ShouldBe("TargetChainId");
        
        entities[1].BlockHeight.ShouldBe(100);
        entities[1].FromChainId.ShouldBe(ChainId);
        entities[1].TransferTransactionId.ShouldBe(logEventContext1.Transaction.TransactionId);
        entities[1].FromAddress.ShouldBe("28vdNy4wFgkan2jFhxshXTnJS5zR2LWxrTBVmKrSYTUWyWVZ8C");
        entities[1].ToAddress.ShouldBe("TargetAddressA");
        entities[1].ReceiptId.ShouldBe("ReceiptIdA");
        entities[1].TransferAmount.ShouldBe(200);
        entities[1].TransferTokenSymbol.ShouldBe("USDT");
        entities[1].TransferType.ShouldBe(TransferType.Transfer);
        entities[1].CrossChainType.ShouldBe(CrossChainType.Heterogeneous);
        entities[1].TransferTime.ShouldBe(logEventContext1.Block.BlockTime);
        entities[1].TransferBlockHeight.ShouldBe(logEventContext1.Block.BlockHeight);
        entities[1].ToChainId.ShouldBe("tDVW");
    }
}