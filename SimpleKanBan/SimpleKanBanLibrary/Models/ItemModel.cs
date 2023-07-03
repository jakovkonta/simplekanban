namespace SimpleKanBanLibrary.Models;

public class ItemModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; }

    public DateTime DateClosed { get; set; }

    public string Author { get; set; }

    public string Responsible { get; set; }

    public StatusModel ItemStatus { get; set; }

    public CategoryModel Category { get; set; }

    public List<string> Comments { get; set; }

    public bool Archived { get; set; }
}
