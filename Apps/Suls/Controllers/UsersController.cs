using SUS.MvcFramework;
using SUS.HTTP;
using Suls.ViewModels.Users;
using Suls.Services;
using System.ComponentModel.DataAnnotations;

namespace Suls.Controllers
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
        public HttpResponse Login(LoginInputModel model)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService.GetUserId(model);
            if(userId == null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);
            return this.Redirect("/");
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
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if(string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 5 || model.Username.Length > 20)
            {
                return this.Error("Invalid username input.Username must be between 5 and 20 characters.");
            }
            if(string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.Error("Invalid password input.Password must be between 6 and 20 characters.");
            }
            if(string.IsNullOrWhiteSpace(model.Email) || !new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Invalid email input.");
            }
            if(model.Password != model.ConfirmPassword)
            {
                return this.Error("Passwords should match.");
            }
            if (!this.usersService.isEmailAvailable(model.Email))
            {
                return this.Error("Email already taken.");
            }
            if (!this.usersService.isUsernameAvailable(model.Username))
            {
                return this.Error("Username already taken.");
            }

            this.usersService.RegisterUser(model);
            return this.Redirect("/");
        }

        public HttpResponse Logout()
        {
            this.SignOut();
            return this.Redirect("/Users/Login");
        }
    }
}
