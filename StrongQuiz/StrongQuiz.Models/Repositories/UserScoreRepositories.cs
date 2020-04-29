using Microsoft.EntityFrameworkCore;
using StrongQuiz.Models.Models;
using StrongQuiz.Web.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public class UserScoreRepositories : IUserScoreRepositories
    {
        private readonly StrongQuizDbContext _context;

        public UserScoreRepositories(StrongQuizDbContext strongQuizDbContext)
        {
            this._context = strongQuizDbContext;
        }
        public async Task<IEnumerable<UserScore>> GetUserScoreAsync() => await _context.UserScores.ToListAsync<UserScore>();
        public async Task<int> GetTimesTakenForEachQuiz(Guid guid)
        {
            var result = _context.UserScores
                            .GroupBy(e => e.QuizId)
                            .Select(e => new { e.Key, Count = e.Count() })
                            .ToDictionary(e => e.Key, e => e.Count);
            int number = result[guid];


            return number;
        }

        public async Task<IEnumerable<UserScore>> GetUserScoreWithUserAsync(Guid quizId)
        {
            var result = await _context.UserScores.Include(e => e.ApplicationUser)
                            .GroupBy(e => new { e.QuizId, e.ApplicationUserId, e.MaxScore})
                            .Select(e => new {Score = e.Max(x=> x.Score), User = e.Key.ApplicationUserId, maxScore = e.Key.MaxScore})
                            .OrderByDescending(e => e.Score)
                            .ToDictionaryAsync(e => new UserScore { Score = e.Score, ApplicationUserId = e.User, MaxScore = e.maxScore});
            List<UserScore> userScores = new List<UserScore>();
            foreach (var item in result)
            {
                UserScore userScore = new UserScore()
                {
                    ApplicationUserId = item.Value.User,
                    Score = item.Value.Score,
                    MaxScore = item.Value.maxScore
                };
                userScores.Add(userScore);
            }
            //await _context.UserScores.Include(p => p.ApplicationUser).Where(p => p.QuizId == quizId).OrderByDescending(p => p.Score).Take(10).ToListAsync()
            return userScores;
        }

        public async Task<UserScore> Add(UserScore userScore)
        {
            try
            {
                var result = _context.UserScores.AddAsync(userScore); //ChangeTracking
                await _context.SaveChangesAsync();//MUST !!!!
                                                  //return result; //NOK	
                return userScore;   //ByRef   -> autoIdentity ingevuld            
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.InnerException.Message);
                return null;
            }
        }
    }
}
