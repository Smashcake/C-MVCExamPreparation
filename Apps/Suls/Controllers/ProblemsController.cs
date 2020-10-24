using SUS.MvcFramework;
using SUS.HTTP;
using Suls.Services;
using Suls.ViewModels.Problems;

namespace Suls.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService problemService;

        public ProblemsController(IProblemsService problemService)
        {
            this.problemService = problemService;
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(CreateProblemInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("Users/Login");
            }

            if(string.IsNullOrWhiteSpace(model.Name) || model.Name.Length < 5 || model.Name.Length > 20)
            {
                return this.Error("Invalid problem name.Problem name should be between 5 and 20 characters.");
            }
            if(model.Points < 50 || model.Points > 300)
            {
                return this.Error("Invalid problem points.Problem points should be between 50 and 300.");
            }

            this.problemService.CreateProblem(model);
            return this.Redirect("/");
        }

        public HttpResponse Details(string id)
        {
            var problemDetails = this.problemService.Details(id);
            return this.View(problemDetails);
        }
    }
}
