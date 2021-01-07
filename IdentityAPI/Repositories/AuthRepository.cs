using AutoMapper;
using IdentityAPI.Interfaces;
using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoPersistence;
using MongoPersistence.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAPI.Repositories
{
    public class AuthService : IDisposable, IAuthRepository
    {
        private readonly AppSettings _appSettings;
        private MongoConnectionManager _manager;
        IMongoCollection<UserEntity> _userCollection;

        public AuthService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _manager = new MongoConnectionManager();
            _userCollection = _manager.GetCollection<UserEntity>("UserDomain");

        }

        public async Task<UserModel> Signup(UserModel userModel)
        {
            //TODO: make safer

            //store in db
            UserEntity userEntity = Mapper.Map<UserEntity>(userModel);
            await _userCollection.InsertOneAsync(userEntity);

            //generate a token
            GenerateToken(userModel);

            //return UserModel
            return userModel;
        }
        public async Task<UserModel> Authenticate(UserModel userFromPostman)
        {
            var userFromDatabase = await _userCollection.Find<UserEntity>(x => x.UserName == userFromPostman.UserName && x.Password == userFromPostman.Password).FirstOrDefaultAsync();
    
            // return null if user not found
            if (userFromDatabase == null || userFromDatabase.UserID == null)
                return null;

            userFromPostman.UserId = userFromDatabase.UserID;
            //TODO: fix async
            return GenerateToken(userFromPostman);
        }
 
        public UserModel GenerateToken(UserModel user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //can have different claims, name maps to userId
                    new Claim("userId", user.UserId.ToString()), 
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.GivenName, user.UserId.ToString())
                }),
                //expires in 7 days
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }
        public async Task<UserEntity> GetUserByIdAsync(UserModel userModel)
        {
            if (userModel == null)
                return null;

            return await _userCollection.Find<UserEntity>(p => p.UserID == userModel.UserId).FirstOrDefaultAsync();
        }

        public async Task<UserEntity> GetUserByNameAsync(UserModel userModel)
        {
            if (userModel == null)
                return null;

            return await _userCollection.Find<UserEntity>(p => p.UserName == userModel.UserName).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
        }
    }
}
