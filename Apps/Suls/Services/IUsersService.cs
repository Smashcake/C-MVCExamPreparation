using Suls.ViewModels.Users;

namespace Suls.Services
{
    public interface IUsersService
    {
        bool isUsernameAvailable(string username);

        bool isEmailAvailable(string email);

        string GetUserId(LoginInputModel model);

        void RegisterUser(RegisterInputModel model);
    }
}
