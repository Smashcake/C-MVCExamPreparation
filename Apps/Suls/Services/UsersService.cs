using Suls.Data;
using Suls.ViewModels.Users;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Suls.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string GetUserId(LoginInputModel model)
        {
            var hashedPassword = ComputeHash(model.Password);
            var user = this.db.Users.Where(u => u.Password == hashedPassword && u.Username == model.Username).FirstOrDefault();
            return user?.Id;
        }

        public bool isEmailAvailable(string email)
        {
            return !this.db.Users.Any(u => u.Email == email); 
        }

        public bool isUsernameAvailable(string username)
        {
            return !this.db.Users.Any(u => u.Username == username);
        }

        public void RegisterUser(RegisterInputModel model)
        {
            var user = new User
            {
                Username = model.Username,
                Password = ComputeHash(model.Password),
                Email = model.Email
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
    }
}
