using SUS.MvcFramework;
using SUS.HTTP;
using Suls.Services;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProblemsService problemsService;

        public HomeController(IProblemsService problemsService) 
        {
            this.problemsService = problemsService;
        }

        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                var problems = this.problemsService.GetAllProblems();
                return this.View(problems, "IndexLoggedIn");
            }

            return this.View();
        }
    }
}
