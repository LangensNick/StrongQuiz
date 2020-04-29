using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> Add(Answer answer);
        Task<IEnumerable<Answer>> GetAnswersAsync();
        Task<IEnumerable<Answer>> GetAnswersByQuestionAsync(Guid guidid);
    }
}