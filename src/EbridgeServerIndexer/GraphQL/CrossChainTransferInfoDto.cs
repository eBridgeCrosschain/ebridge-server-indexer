using AeFinder.Sdk.Dtos;
using EbridgeServerIndexer.Entities;

namespace EbridgeServerIndexer.GraphQL;

public class CrossChainTransferInfoDto : GraphQLDto
{
    public string FromChainId { get; set; }
    public string ToChainId { get; set; }
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public string TransferTransactionId { get; set; }
    public string ReceiveTransactionId { get; set; }
    public DateTime TransferTime { get; set; }
    public long TransferBlockHeight { get; set; }
    public DateTime ReceiveTime { get; set; }
    public long TransferAmount { get; set; }
    public long ReceiveAmount { get; set; }
    public string ReceiptId { get; set; }
    public string TransferTokenSymbol { get; set; }
    public string ReceiveTokenSymbol { get; set; }
    public TransferType TransferType { get; set; }
    public CrossChainType CrossChainType { get; set; }
}