using System;
using AElfIndexer.Client.GraphQL;

namespace CrossChainServer.Indexer.GraphQL;

public class CrossChainServerIndexerSchema : AElfIndexerClientSchema<Query>
{
    public CrossChainServerIndexerSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}