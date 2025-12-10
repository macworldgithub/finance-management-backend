// Services/UserService.cs
using MongoDB.Driver;
using finance_management_backend.Models;
using BCrypt.Net; // ← This is correct

namespace finance_management_backend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User> CreateAsync(User user)
        {
            // FIX: Use BCrypt.Net.BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _users.InsertOneAsync(user);
            user.PasswordHash = ""; // never return the hash
            return user;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null) return false;

            // FIX: Use BCrypt.Net.BCrypt.Verify
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<User?> GetByIdAsync(string id)
            => await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }
}