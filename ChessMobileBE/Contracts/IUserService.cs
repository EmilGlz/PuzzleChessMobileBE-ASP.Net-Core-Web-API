﻿using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;

namespace ChessMobileBE.Contracts
{
    public interface IUserService
    {
        AuthResult Add(UserDTO dto);
        User GetById(string id);
        User GetByPGId(string playGamesId);
        AuthResult Login(string playGamesId);
    }
}
