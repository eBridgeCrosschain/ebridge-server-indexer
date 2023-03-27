using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace CrossChainServer.Indexer.Entities;

public class CrossChainTransferInfoIndex : CrossChainIndexerEntity<string>, IIndexBuild
{
    [Keyword]
    public string FromChainId { get; set; }
    [Keyword]
    public string ToChainId { get; set; }
    [Keyword]
    public string FromAddress { get; set; }
    [Keyword]
    public string ToAddress { get; set; }
    [Keyword]
    public string TransferTransactionId { get; set; }
    [Keyword]
    public string ReceiveTransactionId { get; set; }
    public DateTime TransferTime { get; set; }
    public long TransferBlockHeight { get; set; }
    public DateTime ReceiveTime { get; set; }
    public long TransferAmount { get; set; }
    public long ReceiveAmount { get; set; }
    [Keyword]
    public string ReceiptId { get; set; }
    public string TransferTokenSymbol { get; set; }
    public string ReceiveTokenSymbol { get; set; }
    public TransferType TransferType { get; set; }
    public CrossChainType CrossChainType { get; set; }
}

public enum TransferType
{
    Transfer,
    Receive
}

public enum CrossChainType
{
    Homogeneous,
    Heterogeneous
}