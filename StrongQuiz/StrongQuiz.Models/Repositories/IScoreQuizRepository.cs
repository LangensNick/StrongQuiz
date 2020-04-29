using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public interface IScoreQuizRepository
    {
        Task<ScoreQuiz> Add(ScoreQuiz scoreQuiz);
        Task<IEnumerable<ScoreQuiz>> GetScoreQuizAsync();
        Task<IEnumerable<ScoreQuiz>> GetScoreQuizByQuizAsync(Guid guidid);
        Task<ICollection<ScoreQuiz>> GetScoreQuizByQuizWithUsersAsync(Guid guidid);
    }
}