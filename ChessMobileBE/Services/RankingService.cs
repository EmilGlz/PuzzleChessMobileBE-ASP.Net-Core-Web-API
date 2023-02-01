﻿using ChessMobileBE.Contracts;
using ChessMobileBE.Helpers;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;
using MongoDB.Driver;

namespace ChessMobileBE.Services
{
    public class RankingService : IRankingService
    {
        private IMongoCollection<Rank> _collection;

        public RankingService(IMongoClient client)
        {
            var database = client.GetDatabase("Ranks");
            _collection = database.GetCollection<Rank>("RanksCollection");
        }

        public Rank CheckRank(RankDTO dto)
        {
            var allRanks = _collection.Find(_ => true).ToList().OrderBy(r => r.Value);
            var myRank = allRanks.FirstOrDefault(r => r.UserId == dto.UserId);
            if (myRank == null)
            {

            }
            // TODO check if rankValue is top 10/20/30/.., add to db
            return new Rank();
        }
    }
}