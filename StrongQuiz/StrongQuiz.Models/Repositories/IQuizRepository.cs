using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public interface IQuizRepository
    {
        Task<Quiz> Add(Quiz quiz);
        Task<Quiz> GetQuizByIdAsync(Guid Key);
        Task<IEnumerable<Quiz>> GetQuizzesAsync();
        Task<Quiz> GetQuizQuestionsAnswersAsync(Guid Key);
    }
}