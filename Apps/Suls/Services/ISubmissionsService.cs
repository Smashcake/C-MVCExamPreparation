using Suls.ViewModels.Submissions;

namespace Suls.Services
{
    public interface ISubmissionsService
    {
        void Create(string id,string userId,string code);

        void Delete(string id);
    }
}
