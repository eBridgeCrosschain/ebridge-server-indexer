using AeFinder.App.TestBase;
using EbridgeServerIndexer.Processors.Bridge;
using EbridgeServerIndexer.Processors.CrossChain;
using EbridgeServerIndexer.Processors.Oracle;
using EbridgeServerIndexer.Processors.Report;
using EbridgeServerIndexer.Processors.Token;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace EbridgeServerIndexer;

[DependsOn(
    typeof(AeFinderAppTestBaseModule),
    typeof(EbridgeServerIndexerModule))]
public class EbridgeServerIndexerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AeFinderAppEntityOptions>(options => { options.AddTypes<EbridgeServerIndexerModule>(); });
        
        // Add your Processors.
        context.Services.AddSingleton<ReceiptCreatedProcessor>();
        context.Services.AddSingleton<TokenSwappedProcessor>();
        //ParentChainIndexedProcessor
        context.Services.AddSingleton<ParentChainIndexedProcessor>();
        //SideChainIndexedProcessor
        context.Services.AddSingleton<SideChainIndexedProcessor>();
        //CommitmentRevealedProcessor
        context.Services.AddSingleton<CommitmentRevealedProcessor>();
        //CommittedProcessor
        context.Services.AddSingleton<CommittedProcessor>();
        //QueryCompletedWithAggregationProcessor
        context.Services.AddSingleton<QueryCompletedWithAggregationProcessor>();
        //QueryCompletedWithoutAggregationProcessor
        context.Services.AddSingleton<QueryCompletedWithoutAggregationProcessor>();
        //QueryCreatedProcessor
        context.Services.AddSingleton<QueryCreatedProcessor>();
        //SufficientCommitmentsCollectedProcessor
        context.Services.AddSingleton<SufficientCommitmentsCollectedProcessor>();
        //ReportConfirmedProcessor
        context.Services.AddSingleton<ReportConfirmedProcessor>();
        //ReportProposedProcessor
        context.Services.AddSingleton<ReportProposedProcessor>();
        //CrossChainTransferredProcessor
        context.Services.AddSingleton<CrossChainTransferredProcessor>();
        //CrossChainReceivedProcessor
        context.Services.AddSingleton<CrossChainReceivedProcessor>();
    }
}