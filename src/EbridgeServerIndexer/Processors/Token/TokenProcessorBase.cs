using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace EbridgeServerIndexer.Processors.Token;

public abstract class TokenProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            EbridgeConst.AELF => EbridgeConst.MultiTokenContractAddress,
            EbridgeConst.tDVV => EbridgeConst.MultiTokenContractAddressTDVV,
            EbridgeConst.tDVW => EbridgeConst.MultiTokenContractAddressTDVW,
            _ => string.Empty
        };
    }

}