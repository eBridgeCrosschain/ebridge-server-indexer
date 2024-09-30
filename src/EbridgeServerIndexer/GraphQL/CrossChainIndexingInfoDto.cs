using AeFinder.Sdk.Dtos;

namespace EbridgeServerIndexer.GraphQL;

public class CrossChainIndexingInfoDto : GraphQLDto
{
    public string IndexChainId { get; set; }
    public long IndexBlockHeight { get; set; }
}