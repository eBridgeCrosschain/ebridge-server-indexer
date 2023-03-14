// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: report_contract.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using System.Collections.Generic;
using aelf = global::AElf.CSharp.Core;

namespace AElf.Contracts.Report {

  #region Events
  public partial class ReportProposed : aelf::IEvent<ReportProposed>
  {
    public global::System.Collections.Generic.IEnumerable<ReportProposed> GetIndexed()
    {
      return new List<ReportProposed>
      {
      };
    }

    public ReportProposed GetNonIndexed()
    {
      return new ReportProposed
      {
        RawReport = RawReport,
        RegimentId = RegimentId,
        Token = Token,
        RoundId = RoundId,
        QueryInfo = QueryInfo,
        TargetChainId = TargetChainId,
      };
    }
  }

  public partial class ReportConfirmed : aelf::IEvent<ReportConfirmed>
  {
    public global::System.Collections.Generic.IEnumerable<ReportConfirmed> GetIndexed()
    {
      return new List<ReportConfirmed>
      {
      };
    }

    public ReportConfirmed GetNonIndexed()
    {
      return new ReportConfirmed
      {
        RoundId = RoundId,
        Signature = Signature,
        RegimentId = RegimentId,
        Token = Token,
        IsAllNodeConfirmed = IsAllNodeConfirmed,
        TargetChainId = TargetChainId,
      };
    }
  }

  public partial class OffChainAggregationRegistered : aelf::IEvent<OffChainAggregationRegistered>
  {
    public global::System.Collections.Generic.IEnumerable<OffChainAggregationRegistered> GetIndexed()
    {
      return new List<OffChainAggregationRegistered>
      {
      };
    }

    public OffChainAggregationRegistered GetNonIndexed()
    {
      return new OffChainAggregationRegistered
      {
        Token = Token,
        OffChainQueryInfoList = OffChainQueryInfoList,
        RegimentId = RegimentId,
        ConfigDigest = ConfigDigest,
        AggregateThreshold = AggregateThreshold,
        AggregatorContractAddress = AggregatorContractAddress,
        ChainName = ChainName,
        Register = Register,
        AggregateOption = AggregateOption,
      };
    }
  }

  public partial class MerkleReportNodeAdded : aelf::IEvent<MerkleReportNodeAdded>
  {
    public global::System.Collections.Generic.IEnumerable<MerkleReportNodeAdded> GetIndexed()
    {
      return new List<MerkleReportNodeAdded>
      {
      };
    }

    public MerkleReportNodeAdded GetNonIndexed()
    {
      return new MerkleReportNodeAdded
      {
        Token = Token,
        NodeIndex = NodeIndex,
        NodeRoundId = NodeRoundId,
        AggregatedData = AggregatedData,
      };
    }
  }

  #endregion
}
#endregion

