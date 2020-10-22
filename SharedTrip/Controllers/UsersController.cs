using SharedTrip.Services;
using SharedTrip.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Controllers
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
                return this.Error("You are already logged in.");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserInputModel model)
        {
            if (this.IsUserSignedIn())
            {
                return this.Error("Why are you trying to do a forced post request on a non-available page?");
            }

            var user = this.usersService.GetUserId(model);
            if(user == null)
            {
                return this.Error("Invalid login.");
            }

            this.SignIn(user);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Error("You are already logged in,no point in trying to register.");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(CreateUserInputModel model)
        {
            if (this.IsUserSignedIn())
            {
                return this.Error("You are already logged in,stop messing with my code.");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 5 || model.Username.Length > 20)
            {
                return this.Error("Invalid username input.Username should be between 5 and 20 characters.");
            }
            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6 || model.Username.Length > 20)
            {
                return this.Error("Invalid password input.Password should be between 6 and 20 characters.");
            }
            if (string.IsNullOrWhiteSpace(model.Email) || !new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Invalid email input.");
            }
            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Password and confirmPassword don't match.");
            }
            if (!this.usersService.isEmailAvailable(model.Email))
            {
                return this.Error("Email is already taken.");
            }
            if (!this.usersService.isUsernameAvailable(model.Username))
            {
                return this.Error("Username is already taken.");
            }

            this.usersService.RegisterUser(model);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }
    }
}
