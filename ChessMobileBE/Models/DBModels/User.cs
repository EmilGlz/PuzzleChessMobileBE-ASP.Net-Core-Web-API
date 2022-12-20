﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChessMobileBE.Models.DBModels
{
    public class User
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PlayGamesId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }

        // history
        // profile datas ...
    }
}
