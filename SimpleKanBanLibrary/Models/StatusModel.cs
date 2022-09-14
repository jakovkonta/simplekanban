namespace SimpleKanBanLibrary.Models;

public class StatusModel
{
   [BsonId]
   [BsonRepresentation(BsonType.ObjectId)]
   public string StatusId { get; set; }
   public string StatusName { get; set; }
   public string StatusDescription { get; set; }
}
