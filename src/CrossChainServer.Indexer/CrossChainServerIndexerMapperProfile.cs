using AElf.Contracts.Bridge;
using AElf.Contracts.CrossChain;
using AElf.Contracts.MultiToken;
using AElf.Contracts.Oracle;
using AElfIndexer.Client.Handlers;
using AutoMapper;
using CrossChainServer.Indexer.Entities;
using CrossChainServer.Indexer.GraphQL;

namespace CrossChainServer.Indexer;

public class CrossChainServerIndexerMapperProfile:Profile
{
    public CrossChainServerIndexerMapperProfile()
    {
        CreateMap<LogEventContext, ReceiptCreated>();
        CreateMap<LogEventContext, TokenSwapped>();
        
        CreateMap<LogEventContext, CrossChainReceived>();
        CreateMap<LogEventContext, CrossChainTransferred>();
        CreateMap<LogEventContext, ParentChainIndexed>();
        CreateMap<LogEventContext, SideChainIndexed>();
        
        CreateMap<LogEventContext, CommitmentRevealed>();
        CreateMap<LogEventContext, Committed>();
        CreateMap<LogEventContext, QueryCompletedWithAggregation>();
        CreateMap<LogEventContext, QueryCompletedWithoutAggregation>();
        CreateMap<LogEventContext, QueryCreated>();
        CreateMap<LogEventContext, SufficientCommitmentsCollected>();

        CreateMap<CrossChainIndexingInfoIndex, CrossChainIndexingInfoDto>();
        CreateMap<CrossChainTransferInfoIndex, CrossChainTransferInfoDto>();
        CreateMap<OracleQueryInfoIndex, OracleQueryInfoDto>();
        CreateMap<ReportInfoIndex, ReportInfoDto>();
        
        CreateMap<LogEventContext, CrossChainIndexingInfoIndex>();
        CreateMap<LogEventContext, CrossChainTransferInfoIndex>();
        CreateMap<LogEventContext, OracleQueryInfoIndex>();
        CreateMap<LogEventContext, ReportInfoIndex>();
    }
}