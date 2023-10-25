using EjerciciosProgramacion.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EjerciciosProgramacion.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userModel;

        public UserService(MongoClient client)
        {
            _userModel = client.GetDatabase("csharp").GetCollection<User>("users");
        }

        public async Task CreateAsync(User user) => await _userModel.InsertOneAsync(user);
    }
}
