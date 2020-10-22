using SharedTrip.ViewModels;

namespace SharedTrip.Services
{
    public interface IUsersService
    {
        bool isEmailAvailable(string email);

        bool isUsernameAvailable(string username);

        void RegisterUser(CreateUserInputModel model);

        string GetUserId(LoginUserInputModel model);
    }
}
