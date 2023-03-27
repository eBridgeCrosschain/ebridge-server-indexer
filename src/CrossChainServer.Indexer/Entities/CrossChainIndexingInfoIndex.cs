using System;
using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace CrossChainServer.Indexer.Entities;

public class CrossChainIndexingInfoIndex : CrossChainIndexerEntity<string>, IIndexBuild
{
    [Keyword]
    public string IndexChainId { get; set; }
    public long IndexBlockHeight { get; set; }
}