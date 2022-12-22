using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChessMobileBE.Models
{
    public class TokenDBModel
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string RefreshId { get; set; }
        public string AccessToken { get; set; }
        public string UserId { get; set; } // Linked to the AspNet Identity User Id
        public DateTime CreationDate { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public DateTime AccessTokenExpiryDate { get; set; }
        public bool IsUsed { get; set; } // if its used we dont want generate a new Jwt token with the same refresh token
        public bool IsRevoked { get; set; } // if it has been revoke for security reasons
    }
}
