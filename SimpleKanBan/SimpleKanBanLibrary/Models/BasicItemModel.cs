namespace SimpleKanBanLibrary.Models;

public class BasicItemModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Title { get; set; }

    public BasicItemModel()
    {
            
    }

    public BasicItemModel(ItemModel item)
    {
        Id = item.Id;
        Title = item.Title;
    }
}
