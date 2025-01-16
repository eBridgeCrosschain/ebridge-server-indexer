using AeFinder.Sdk.Entities;
using Nest;

namespace EbridgeServerIndexer.Entities;

public class CrossChainIndexingInfoIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string IndexChainId { get; set; }
    public long IndexBlockHeight { get; set; }
}