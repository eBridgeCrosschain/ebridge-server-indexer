using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace EbridgeServerIndexer.Processors.Bridge;

public abstract class BridgeProcessorBase<TEvent>: LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            EbridgeConst.AELF => EbridgeConst.BridgeContractAddress,
            EbridgeConst.tDVV => EbridgeConst.BridgeContractAddressTDVV,
            EbridgeConst.tDVW => EbridgeConst.BridgeContractAddressTDVW,
            _ => string.Empty
        };
    }
}