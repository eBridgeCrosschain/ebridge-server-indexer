using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using CrossChainServer.Indexer.GraphQL;
using CrossChainServer.Indexer.Processors.Bridge;
using CrossChainServer.Indexer.Processors.CrossChain;
using CrossChainServer.Indexer.Processors.Oracle;
using CrossChainServer.Indexer.Processors.Report;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace CrossChainServer.Indexer;

[DependsOn(typeof(AElfIndexerClientModule))]
public class CrossChainServerIndexerModule:AElfIndexerClientPluginBaseModule<CrossChainServerIndexerModule, CrossChainServerIndexerSchema, Query>
{
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.GetConfiguration();
        
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ReceiptCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TokenSwappedProcessor>();
        
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CrossChainReceivedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CrossChainTransferredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ParentChainIndexedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SideChainIndexedProcessor>();
        
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CommitmentRevealedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CommittedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, QueryCompletedWithAggregationProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, QueryCompletedWithoutAggregationProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, QueryCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SufficientCommitmentsCollectedProcessor>();
        
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ReportConfirmedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ReportProposedProcessor>();

        Configure<ContractInfoOptions>(configuration.GetSection("ContractInfo"));
    }

    protected override string ClientId => "AElfIndexer_DApp";
    protected override string Version => "";
}