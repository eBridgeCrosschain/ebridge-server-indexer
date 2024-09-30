using AeFinder.Sdk.Processor;
using EbridgeServerIndexer.GraphQL;
using EbridgeServerIndexer.Processors.Bridge;
using EbridgeServerIndexer.Processors.CrossChain;
using EbridgeServerIndexer.Processors.Oracle;
using EbridgeServerIndexer.Processors.Report;
using EbridgeServerIndexer.Processors.Token;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace EbridgeServerIndexer;

public class EbridgeServerIndexerModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<EbridgeServerIndexerModule>(); });
        context.Services.AddSingleton<ISchema, AeIndexerSchema>();
        
        // Add your LogEventProcessor implementation.
        context.Services.AddSingleton<ILogEventProcessor, ReceiptCreatedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, TokenSwappedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, CrossChainTransferredProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, CrossChainReceivedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, ParentChainIndexedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, SideChainIndexedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, CommitmentRevealedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, CommittedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, QueryCompletedWithAggregationProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, QueryCompletedWithoutAggregationProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, QueryCreatedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, SufficientCommitmentsCollectedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, ReportConfirmedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, ReportProposedProcessor>();
    }
}