using AElf;
using AElf.Contracts.Bridge;
using AElf.Contracts.CrossChain;
using AElf.Contracts.MultiToken;
using AElf.Contracts.Oracle;
using AElf.Contracts.Report;
using AElf.Types;
using AElfIndexer.Client.Handlers;
using AutoMapper;
using CrossChainServer.Indexer.Entities;
using CrossChainServer.Indexer.GraphQL;

namespace CrossChainServer.Indexer;

public class CrossChainServerIndexerMapperProfile:Profile
{
    public CrossChainServerIndexerMapperProfile()
    {
        // Common
        CreateMap<Hash, string>().ConvertUsing(s => s == null ? null : s.ToHex());
        CreateMap<Address, string>().ConvertUsing(s => s.ToBase58());
        
        // Token
        CreateMap<LogEventContext, CrossChainTransferInfoIndex>();
        
        CreateMap<CrossChainReceived, CrossChainTransferInfoIndex>()
            .ForMember(d => d.FromChainId, opt => opt.MapFrom(o => ChainHelper.ConvertChainIdToBase58(o.FromChainId)))
            .ForMember(d => d.TransferTransactionId, opt => opt.MapFrom(o => o.TransferTransactionId.ToHex()))
            .ForMember(d => d.ReceiveAmount, opt => opt.MapFrom(o => o.Amount))
            .ForMember(d => d.ReceiveTokenSymbol, opt => opt.MapFrom(o => o.Symbol))
            .ForMember(d => d.FromAddress, opt => opt.MapFrom(o => o.From.ToBase58()))
            .ForMember(d => d.ToAddress, opt => opt.MapFrom(o => o.To.ToBase58()));

        CreateMap<CrossChainTransferred, CrossChainTransferInfoIndex>()
            .ForMember(d => d.TransferAmount, opt => opt.MapFrom(o => o.Amount))
            .ForMember(d => d.TransferTokenSymbol, opt => opt.MapFrom(o => o.Symbol))
            .ForMember(d => d.ToChainId, opt => opt.MapFrom(o => ChainHelper.ConvertChainIdToBase58(o.ToChainId)))
            .ForMember(d => d.FromAddress, opt => opt.MapFrom(o => o.From.ToBase58()))
            .ForMember(d => d.ToAddress, opt => opt.MapFrom(o => o.To.ToBase58()));

        // Report
        CreateMap<LogEventContext, ReportInfoIndex>();
        CreateMap<ReportConfirmed, ReportInfoIndex>();

        CreateMap<ReportProposed, ReportInfoIndex>()
            .ForMember(d => d.ReceiptHash, opt => opt.MapFrom(o => o.QueryInfo.Options[0]))
            .ForMember(d => d.ReceiptInfo, opt => opt.MapFrom(o => o.QueryInfo.Options[1]));

        CreateMap<ReportInfoIndex, ReportInfoDto>();

        // Oracle
        CreateMap<LogEventContext, OracleQueryInfoIndex>();

        CreateMap<CommitmentRevealed, OracleQueryInfoIndex>();
        CreateMap<Committed, OracleQueryInfoIndex>();
        CreateMap<QueryCompletedWithAggregation, OracleQueryInfoIndex>();
        CreateMap<QueryCompletedWithoutAggregation, OracleQueryInfoIndex>();
        CreateMap<SufficientCommitmentsCollected, OracleQueryInfoIndex>();
        
        CreateMap<OracleQueryInfoIndex, OracleQueryInfoDto>();

        // CrossChain
        CreateMap<ParentChainIndexed, CrossChainIndexingInfoIndex>()
            .ForMember(d => d.IndexChainId, opt => opt.MapFrom(o => ChainHelper.ConvertChainIdToBase58(o.ChainId)))
            .ForMember(d => d.IndexBlockHeight, opt => opt.MapFrom(o => o.IndexedHeight));

        CreateMap<SideChainIndexed, CrossChainIndexingInfoIndex>()
            .ForMember(d => d.IndexChainId, opt => opt.MapFrom(o => ChainHelper.ConvertChainIdToBase58(o.ChainId)))
            .ForMember(d => d.IndexBlockHeight, opt => opt.MapFrom(o => o.IndexedHeight));

        CreateMap<CrossChainIndexingInfoIndex, CrossChainIndexingInfoDto>();
        CreateMap<CrossChainTransferInfoIndex, CrossChainTransferInfoDto>();
        
        CreateMap<LogEventContext, CrossChainIndexingInfoIndex>();

        // Bridge
        CreateMap<ReceiptCreated, CrossChainTransferInfoIndex>()
            .ForMember(d => d.TransferAmount, opt => opt.MapFrom(o => o.Amount))
            .ForMember(d => d.FromAddress, opt => opt.MapFrom(o => o.Owner.ToBase58()))
            .ForMember(d => d.ToAddress, opt => opt.MapFrom(o => o.TargetAddress))
            .ForMember(d => d.TransferTokenSymbol, opt => opt.MapFrom(o => o.Symbol))
            .ForMember(d => d.ToChainId, opt => opt.MapFrom(o => o.TargetChainId));

        CreateMap<TokenSwapped, CrossChainTransferInfoIndex>()
            .ForMember(d => d.ReceiveAmount, opt => opt.MapFrom(o => o.Amount))
            .ForMember(d => d.ReceiveTokenSymbol, opt => opt.MapFrom(o => o.Symbol))
            .ForMember(d => d.ToAddress, opt => opt.MapFrom(o => o.Address.ToBase58()));
    }
}