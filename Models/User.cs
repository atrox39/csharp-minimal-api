using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EjerciciosProgramacion.Models
{
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
  }
}
