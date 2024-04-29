using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Taka.App.Authentication.Domain.Entities
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserEmail { get; set; }
        public string Token { get; set; }       
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && Expires > DateTime.UtcNow;
    }
}
