using SharedTrip.Data;
using SharedTrip.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharedTrip.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool isEmailAvailable(string email)
        {
            return !this.db.Users.Any(u => u.Email == email);
        }

        public bool isUsernameAvailable(string username)
        {
            return !this.db.Users.Any(u => u.Username == username);
        }

        public void RegisterUser(CreateUserInputModel model)
        {
            var user = new User()
            {
                Username = model.Username,
                Email = model.Email,
                Password = ComputeHash(model.Password)
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

        public string GetUserId(LoginUserInputModel model)
        {
            var hashedPassword = ComputeHash(model.Password);
            var user = this.db.Users
                .Where(u => u.Username == model.Username && u.Password == hashedPassword)
                .FirstOrDefault();

            return user?.Id;
        }
    }
}
