using Microsoft.Extensions.Caching.Memory;

namespace SimpleKanBanLibrary.DataAccess;

public class MongoStatusData : IStatusData
{
    private readonly IMongoCollection<StatusModel> _statuses;
    private readonly IMemoryCache _cache;
    private const string _cacheName = "StatusData";

    public MongoStatusData(IDbConnection db, IMemoryCache cache)
    {
        _statuses = db.StatusCollection;
        _cache = cache;
    }

    public async Task<List<StatusModel>> GetAllStatuses()
    {
        var output = _cache.Get<List<StatusModel>>(_cacheName);
        if (output == null)
        {
            var result = await _statuses.FindAsync(_ => true);
            output = result.ToList();

            _cache.Set(_cacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }

    public Task CreateStatus(StatusModel status)
    {
        return _statuses.InsertOneAsync(status);
    }
}
