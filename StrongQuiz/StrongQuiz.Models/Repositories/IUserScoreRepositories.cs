using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public interface IUserScoreRepositories
    {
        Task<UserScore> Add(UserScore userScore);
        Task<int> GetTimesTakenForEachQuiz(Guid guid);
        Task<IEnumerable<UserScore>> GetUserQuizzesAsync(string guid);
        Task<IEnumerable<UserScore>> GetUserScoreAsync();
        Task<IEnumerable<UserScore>> GetUserScoreWithUserAsync(Guid quizId);
    }
}