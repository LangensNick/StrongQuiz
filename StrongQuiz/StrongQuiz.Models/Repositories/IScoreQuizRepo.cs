using StrongQuiz.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public interface IScoreQuizRepo
    {
        Task<ScoreQuiz> Add(ScoreQuiz scoreQuiz);
        Task<IEnumerable<ScoreQuiz>> GetScoreQuizzesAsync();
    }
}