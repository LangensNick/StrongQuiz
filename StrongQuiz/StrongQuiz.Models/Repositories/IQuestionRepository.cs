using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question> Add(Question question);
        Task<Question> GetQuestionByIdAsync(Guid Key);
        Task<IEnumerable<Question>> GetQuestionsAsync();
        Task<Question> GetQuestionsAndAnswersAsync(Guid guid);
    }
}