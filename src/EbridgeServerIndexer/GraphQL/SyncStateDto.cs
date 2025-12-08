
namespace EbridgeServerIndexer.GraphQL;

public class SyncStateDto : GraphQLDto
{
    public long ConfirmedBlockHeight { get; set; }
}