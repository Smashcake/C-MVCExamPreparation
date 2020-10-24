using BattleCards.Services;
using BattleCards.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System.ComponentModel.DataAnnotations;

namespace BattleCards.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {

            var userId = this.usersService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);
            return this.Redirect("/Cards/All");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Error("You have to be logged in in order to logout.");
            }

            this.SignOut();
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 5 || model.Username.Length > 20)
            {
                return this.Error("Invalid username input.Username should be between 5 and 20 characters.");
            }
            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.Error("Invalid password input.Password should be between 6 and 20 characters.");
            }
            if (string.IsNullOrWhiteSpace(model.Email) || !new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Invalid email input.");
            }
            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Passwords should match.");
            }
            if (!this.usersService.IsUsernameAvailable(model.Username))
            {
                return this.Error("Username or email already taken.");
            }
            if (!this.usersService.IsEmailAvailable(model.Email))
            {
                return this.Error("Username or email already taken.");
            }

            this.usersService.CreateUser(model.Username, model.Password, model.Email);
            return this.Redirect("/Users/Login");
        }
    }
}