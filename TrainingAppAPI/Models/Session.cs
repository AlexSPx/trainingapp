using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainingAppAPI.Models;

public class Session {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}
    public string? UserId {get; set;}

    [BsonElement("expiry")]
    public DateTime ExpireAt {get; set;}
}