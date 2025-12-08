using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace EbridgeServerIndexer.Processors.Report;

public abstract class ReportProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            EbridgeConst.AELF => EbridgeConst.ReportContractAddress,
            EbridgeConst.tDVV => EbridgeConst.ReportContractAddressTDVV,
            EbridgeConst.tDVW => EbridgeConst.ReportContractAddressTDVW,
            _ => string.Empty
        };
    }
}