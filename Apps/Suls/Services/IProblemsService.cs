using Suls.ViewModels.Problems;
using System.Collections.Generic;

namespace Suls.Services
{
    public interface IProblemsService
    {
        IEnumerable<AllProblemsViewModel> GetAllProblems();

        void CreateProblem(CreateProblemInputModel model);

        ProblemDetailsViewModel Details(string problemId);

        string GetNameById(string id);
    }
}
