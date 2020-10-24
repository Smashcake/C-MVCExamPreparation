using Suls.Data;
using Suls.ViewModels.Problems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suls.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly ApplicationDbContext db;
        private readonly Random random;

        public ProblemsService(ApplicationDbContext db,Random random)
        {
            this.db = db;
            this.random = random;
        }

        public void CreateProblem(CreateProblemInputModel model)
        {
            var problem = new Problem
            {
                Name = model.Name,
                Points = (ushort)model.Points
            };

            this.db.Problems.Add(problem);
            this.db.SaveChanges();
        }

        public ProblemDetailsViewModel Details(string problemId)
        {
            var problemDetails = this.db.Problems.Where(p => p.Id == problemId).Select(p => new ProblemDetailsViewModel
            {
                Name = p.Name,
                Submissions = this.db.Submissions.Where(s => s.ProblemId == problemId).Select(s => new ProblemSubmissionViewModel
                {
                    MaxPoints = p.Points,
                    SubmissionId = s.Id,
                    Username = s.User.Username,
                    CreatedOn = s.CreatedOn,
                    AchievedResult = s.AchievedResult
                }).ToList()
            }
            ).FirstOrDefault();

            return problemDetails;
        }

        public IEnumerable<AllProblemsViewModel> GetAllProblems()
        {
            var problems = this.db.Problems.Select(p => new AllProblemsViewModel
            {
                Count = this.db.Submissions.Where(s => s.ProblemId == p.Id).Count(),
                Name = p.Name,
                Id = p.Id
            }).ToList();

            return problems;
        }

        public string GetNameById(string id)
        {
            var problem = this.db.Problems.FirstOrDefault(p => p.Id == id);
            return problem?.Name;
        }
    }
}
