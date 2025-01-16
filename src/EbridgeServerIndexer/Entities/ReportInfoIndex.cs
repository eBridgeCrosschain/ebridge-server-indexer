using AeFinder.Sdk.Entities;
using JetBrains.Annotations;
using Nest;

namespace EbridgeServerIndexer.Entities;

public class ReportInfoIndex : AeFinderEntity, IAeFinderEntity
{
    public long RoundId { get; set; }
    [Keyword] public string Token { get; set; }
    [Keyword] public string TargetChainId { get; set; }
    [Keyword] public string ReceiptId { get; set; }
    [Keyword] public string ReceiptHash { get; set; }
    public ReportStep Step { get; set; }

    [Keyword] [CanBeNull] public string ReceiptInfo { get; set; }
}

public enum ReportStep
{
    Proposed = 0,
    Confirmed = 1
}