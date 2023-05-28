using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Driver;
using TrainingAppAPI.Models;

namespace TrainingAppAPI.Services;

public interface IUserService
{ 
    Task<User> GetUserById(string id);
    Task<User> CreateUser(User user);
    Task<User> FindUserByEmail(string email);
}


public class UserService : IUserService {
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(DatabaseService DBService) {
        _usersCollection = DBService.Users;
    }

    public async Task<User> CreateUser(User user) {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: user.Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
        await _usersCollection.InsertOneAsync(user);
        return user;
    } 

    public async Task<User> FindUserByEmail(string email) =>
        await _usersCollection.Find(users => users.Email == email).FirstOrDefaultAsync();

    public async Task<User> GetUserById(string id) =>
        await _usersCollection.Find(users => users.Id == id).FirstOrDefaultAsync();
}