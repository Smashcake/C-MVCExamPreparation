using BattleCards.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BattleCards.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void CreateUser(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = ComputeHash(password)
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.db.Users.Any(u => u.Username == username);
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db.Users.Any(u => u.Email == email);
        }

        public string GetUserId(string username, string password)
        {
            var hashedPassword = ComputeHash(password);
            var user = this.db.Users.Where(u => u.Username == username && u.Password == hashedPassword).FirstOrDefault();
            return user?.Id;
        }
    }
}
