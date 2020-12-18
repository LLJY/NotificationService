using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.Models
{
    public class UserToken
    {
        // user id from firebase cloud storage
        public string Id { get; set; }
        // notification token
        public string Token { get; set; }
    }
}