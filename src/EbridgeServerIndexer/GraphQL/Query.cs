using AeFinder.Sdk;
using EbridgeServerIndexer.Entities;
using GraphQL;
using Volo.Abp.ObjectMapping;
using QueryInput = EbridgeServerIndexer.GraphQL.QueryInput;

namespace EbridgeServerIndexer.GraphQL;

public class Query
{
    public static async Task<List<ReportInfoDto>> ReportInfo(
        [FromServices] IReadOnlyRepository<ReportInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        QueryInput input)
    {
        input.Validate();

        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);

        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight >= input.StartBlockHeight);
        }

        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight <= input.EndBlockHeight);
        }

        var result = queryable.OrderBy(o => o.Metadata.Block.BlockHeight).Take(input.MaxMaxResultCount).ToList();
        return objectMapper.Map<List<ReportInfoIndex>, List<ReportInfoDto>>(result);
    }

    public static async Task<List<CrossChainIndexingInfoDto>> CrossChainIndexingInfo(
        [FromServices] IReadOnlyRepository<CrossChainIndexingInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        QueryInput input)
    {
        input.Validate();

        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);

        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight >= input.StartBlockHeight);
        }

        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight <= input.EndBlockHeight);
        }

        var result = queryable.OrderBy(o => o.Metadata.Block.BlockHeight).Take(input.MaxMaxResultCount).ToList();
        return objectMapper.Map<List<CrossChainIndexingInfoIndex>, List<CrossChainIndexingInfoDto>>(result);
    }

    public static async Task<List<CrossChainTransferInfoDto>> CrossChainTransferInfo(
        [FromServices] IReadOnlyRepository<CrossChainTransferInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper, QueryInput input)
    {
        input.Validate();

        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);

        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight >= input.StartBlockHeight);
        }

        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight <= input.EndBlockHeight);
        }

        var result = queryable.OrderBy(o => o.Metadata.Block.BlockHeight).Take(input.MaxMaxResultCount).ToList();
        return objectMapper.Map<List<CrossChainTransferInfoIndex>, List<CrossChainTransferInfoDto>>(result);
    }

    public static async Task<List<OracleQueryInfoDto>> OracleQueryInfo(
        [FromServices] IReadOnlyRepository<OracleQueryInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper, QueryInput input)
    {
        input.Validate();

        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);

        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight >= input.StartBlockHeight);
        }

        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight <= input.EndBlockHeight);
        }

        var result = queryable.OrderBy(o => o.Metadata.Block.BlockHeight).Take(input.MaxMaxResultCount).ToList();
        return objectMapper.Map<List<OracleQueryInfoIndex>, List<OracleQueryInfoDto>>(result);
    }

    public static async Task<CrossChainTransferInfoDto> HomogeneousCrossChainTransferInfo(
        [FromServices] IReadOnlyRepository<CrossChainTransferInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCrossChainInfoInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        queryable = queryable.Where(a => a.TransferTransactionId == input.TransactionId);
        var result = queryable.FirstOrDefault();
        return result == null ? null : objectMapper.Map<CrossChainTransferInfoIndex, CrossChainTransferInfoDto>(result);
    }
}