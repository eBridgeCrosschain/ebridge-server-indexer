using AeFinder.Sdk;
using AeFinder.Sdk.Entities;

namespace EbridgeServerIndexer;

public class EbridgeServerTestsHelper
{
    static public async Task<TEntity> GetEntityAsync<TEntity>(IReadOnlyRepository<TEntity> repository, string id) where TEntity : AeFinderEntity
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Id == id);
        return queryable.ToList()[0];
    }
}