using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentication.Domain.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("AccessibleMicroservices")]
        public List<Microservices> AccessibleMicroservices { get; set; } = new List<Microservices> { Microservices.ServicesRental, Microservices.ServicesDeliveryPerson, Microservices.ServicesMotorcycle};

        [BsonElement("Roles")]        
        public List<RolesTypes> Roles { get; set; } = new List<RolesTypes> { RolesTypes.DeliveryPerson };

        [BsonElement("Active")]
        public bool Active { get; set; }  = true;
    }
}
