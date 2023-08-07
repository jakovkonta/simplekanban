using MongoDB.Driver;

namespace SimpleKanBanLibrary.DataAccess;
public interface IDbConnection
{
    IMongoCollection<CategoryModel> CategoryCollection { get; }
    string CategoryCollectionName { get; }
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<ItemModel> ItemCollection { get; }
    string ItemCollectionName { get; }
    IMongoCollection<StatusModel> StatusCollection { get; }
    string StatusCollectionName { get; }
    IMongoCollection<UserModel> UserCollection { get; }
    string UserCollectionName { get; }
}