using AElfIndexer.Client.GraphQL;

namespace CrossChainServer.Indexer.GraphQL;

public class CrossChainServerIndexerCASchema : AElfIndexerClientSchema<Query>
{
    public CrossChainServerIndexerCASchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}