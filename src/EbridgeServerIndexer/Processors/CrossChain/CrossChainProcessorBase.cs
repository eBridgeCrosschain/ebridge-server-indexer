using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace EbridgeServerIndexer.Processors.CrossChain;

public abstract class CrossChainProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            EbridgeConst.AELF => EbridgeConst.CrossChainContractAddress,
            EbridgeConst.tDVV => EbridgeConst.CrossChainContractAddressTDVV,
            EbridgeConst.tDVW => EbridgeConst.CrossChainContractAddressTDVW,
            _ => string.Empty
        };
    }
}