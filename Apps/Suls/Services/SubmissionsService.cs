using Suls.Data;
using Suls.ViewModels.Submissions;
using System;
using System.Linq;

namespace Suls.Services
{
    public class SubmissionsService : ISubmissionsService
    {
        private readonly ApplicationDbContext db;
        private readonly Random random;

        public SubmissionsService(ApplicationDbContext db, Random random)
        {
            this.db = db;
            this.random = random;
        }

        public void Create(string id, string userId, string code)
        {
            var problem = this.db.Problems.FirstOrDefault(p => p.Id == id);

            var submission = new Submission()
            {
                ProblemId = id,
                UserId = userId,
                Code = code,
                CreatedOn = DateTime.UtcNow,
                AchievedResult = (ushort)random.Next(0,problem.Points + 1)
            };

            this.db.Submissions.Add(submission);
            this.db.SaveChanges();
        }

        public void Delete(string id)
        {
            var submission = this.db.Submissions.FirstOrDefault(s => s.Id == id);
            this.db.Submissions.Remove(submission);
            this.db.SaveChanges();
        }
    }
}
