using AeFinder.Sdk.Processor;
using AElf;
using AElf.Contracts.CrossChain;
using AElf.Contracts.MultiToken;
using EBridge.Contracts.Report;
using AElf.Types;
using EbridgeServerIndexer.Entities;
using EbridgeServerIndexer.GraphQL;
using AutoMapper;
using EBridge.Contracts.Bridge;
using EBridge.Contracts.Oracle;
using EBridge.Contracts.TokenPool;

namespace EbridgeServerIndexer;

public class EbridgeServerIndexerProfile : Profile
{
    public EbridgeServerIndexerProfile()
    {
       // Common
        CreateMap<Hash, string>().ConvertUsing(s => s == null ? null : s.ToHex());
        CreateMap<Address, string>().ConvertUsing(s => s.ToBase58());
        
        // Token
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
        CreateMap<ReportConfirmed, ReportInfoIndex>();

        CreateMap<ReportProposed, ReportInfoIndex>()
            .ForMember(d => d.ReceiptHash, opt => opt.MapFrom(o => o.QueryInfo.Options[0]));

        CreateMap<ReportInfoIndex, ReportInfoDto>()
            .ForMember(d=>d.BlockHash, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHash))
            .ForMember(d=>d.BlockHeight, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHeight))
            .ForMember(d=>d.BlockTime, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockTime))
            .ForMember(d=>d.ChainId, opt=>opt.MapFrom(o=>o.Metadata.ChainId));
        // Oracle
        CreateMap<CommitmentRevealed, OracleQueryInfoIndex>();
        CreateMap<Committed, OracleQueryInfoIndex>();
        CreateMap<QueryCompletedWithAggregation, OracleQueryInfoIndex>();
        CreateMap<QueryCompletedWithoutAggregation, OracleQueryInfoIndex>();
        CreateMap<SufficientCommitmentsCollected, OracleQueryInfoIndex>();
        
        CreateMap<OracleQueryInfoIndex, OracleQueryInfoDto>()
            .ForMember(d=>d.BlockHash, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHash))
            .ForMember(d=>d.BlockHeight, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHeight))
            .ForMember(d=>d.BlockTime, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockTime))
            .ForMember(d=>d.ChainId, opt=>opt.MapFrom(o=>o.Metadata.ChainId));
        // CrossChain
        CreateMap<ParentChainIndexed, CrossChainIndexingInfoIndex>()
            .ForMember(d => d.IndexChainId, opt => opt.MapFrom(o => ChainHelper.ConvertChainIdToBase58(o.ChainId)))
            .ForMember(d => d.IndexBlockHeight, opt => opt.MapFrom(o => o.IndexedHeight));

        CreateMap<SideChainIndexed, CrossChainIndexingInfoIndex>()
            .ForMember(d => d.IndexChainId, opt => opt.MapFrom(o => ChainHelper.ConvertChainIdToBase58(o.ChainId)))
            .ForMember(d => d.IndexBlockHeight, opt => opt.MapFrom(o => o.IndexedHeight));

        CreateMap<CrossChainIndexingInfoIndex, CrossChainIndexingInfoDto>()
            .ForMember(d=>d.BlockHash, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHash))
            .ForMember(d=>d.BlockHeight, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHeight))
            .ForMember(d=>d.BlockTime, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockTime))
            .ForMember(d=>d.ChainId, opt=>opt.MapFrom(o=>o.Metadata.ChainId));
        CreateMap<CrossChainTransferInfoIndex, CrossChainTransferInfoDto>()
            .ForMember(d=>d.BlockHash, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHash))
            .ForMember(d=>d.BlockHeight, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHeight))
            .ForMember(d=>d.BlockTime, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockTime))
            .ForMember(d=>d.ChainId, opt=>opt.MapFrom(o=>o.Metadata.ChainId));
        
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
        // TokenPool
        CreateMap<UserLiquidityRecordIndex, UserLiquidityRecordDto>()
            .ForMember(d=>d.BlockHash, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHash))
            .ForMember(d=>d.BlockHeight, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHeight))
            .ForMember(d=>d.BlockTime, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockTime))
            .ForMember(d=>d.ChainId, opt=>opt.MapFrom(o=>o.Metadata.ChainId));
        CreateMap<PoolLiquidityRecordIndex, PoolLiquidityRecordDto>()
            .ForMember(d=>d.BlockHash, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHash))
            .ForMember(d=>d.BlockHeight, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockHeight))
            .ForMember(d=>d.BlockTime, opt=>opt.MapFrom(o=>o.Metadata.Block.BlockTime))
            .ForMember(d=>d.ChainId, opt=>opt.MapFrom(o=>o.Metadata.ChainId));
    }
}