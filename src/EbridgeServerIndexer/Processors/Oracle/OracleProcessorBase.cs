using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace EbridgeServerIndexer.Processors.Oracle;

public abstract class OracleProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            EbridgeConst.AELF => EbridgeConst.OracleContractAddress,
            EbridgeConst.tDVV => EbridgeConst.OracleContractAddressTDVV,
            EbridgeConst.tDVW => EbridgeConst.OracleContractAddressTDVW,
            _ => string.Empty
        };
    }
}