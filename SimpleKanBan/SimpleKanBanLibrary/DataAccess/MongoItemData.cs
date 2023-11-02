using Amazon.Runtime.EventStreams;
using Microsoft.Extensions.Caching.Memory;

namespace SimpleKanBanLibrary.DataAccess;

public class MongoItemData : IItemData
{
    private readonly IDbConnection _db;
    private readonly IUserData _userData;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<ItemModel> _items;
    private const string CacheName = "ItemData";

    public MongoItemData(IDbConnection db, IUserData userData, IMemoryCache cache)
    {
        _db = db;
        _userData = userData;
        _cache = cache;
        _items = db.ItemCollection;
    }

    public async Task<List<ItemModel>> GetAllItems()
    {
        var output = _cache.Get<List<ItemModel>>(CacheName);

        if (output == null)
        {
            var result = await _items.FindAsync(i => i.Archived == false);
            output = result.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }

        return output;
    }

    public async Task<List<ItemModel>> GetAllUserItems(string userId)
    {
        var output = _cache.Get<List<ItemModel>>(userId);

        if (output is null)
        {
            var result = await _items.FindAsync(i => i.Author.Id == userId);
            output = result.ToList();

            _cache.Set(userId, output, TimeSpan.FromMinutes(1));
        }

        return output;
    }

    public async Task<ItemModel> GetItem(string id)
    {
        var result = await _items.FindAsync(i => i.Id == id);

        return result.FirstOrDefault();
    }

    public async Task UpdateItem(ItemModel item)
    {
        await _items.ReplaceOneAsync(i => i.Id == item.Id, item);
        _cache.Remove(CacheName);
    }

    public async Task CreateItem(ItemModel item)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var itemsInTransaction = db.GetCollection<ItemModel>(_db.ItemCollectionName);
            await itemsInTransaction.InsertOneAsync(item);

            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(item.Author.Id);
            user.AuthoredItems.Add(new BasicItemModel(item));
            await usersInTransaction.ReplaceOneAsync(i => i.Id == user.Id, user);

            await session.CommitTransactionAsync();
            _cache.Remove(CacheName);
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }

    }
}
