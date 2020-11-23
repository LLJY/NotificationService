using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.Models
{
    public class UserToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        // user id from firebase cloud storage
        public string Id { get; set; }
        // notification token
        public string Token { get; set; }
    }
}