namespace SimpleKanBanLibrary.DataAccess;

public interface IItemData
{
    Task CreateItem(ItemModel item);
    Task<List<ItemModel>> GetAllItems();
    Task<List<ItemModel>> GetAllUserItems(string userId);
    Task<ItemModel> GetItem(string id);
    Task UpdateItem(ItemModel item);
}