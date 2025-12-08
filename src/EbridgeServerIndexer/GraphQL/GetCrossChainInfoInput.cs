using System.ComponentModel.DataAnnotations;

namespace EbridgeServerIndexer.GraphQL;

public class GetCrossChainInfoInput
{
    public string ChainId { get; set; }
    public string TransactionId { get; set; }
}