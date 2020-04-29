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
    public class AnswerRepository : IAnswerRepository
    {
        private readonly StrongQuizDbContext _context;

        public AnswerRepository(StrongQuizDbContext strongQuizDbContext)
        {
            this._context = strongQuizDbContext;
        }
        public async Task<IEnumerable<Answer>> GetAnswersAsync() => await _context.Answers.ToListAsync<Answer>();
        public async Task<IEnumerable<Answer>> GetAnswersByQuestionAsync(Guid guidid) => await _context.Answers.Where(b => b.QuestionId == guidid).ToListAsync<Answer>();
        
        public async Task<Answer> Add(Answer answer)
        {
            try
            {
                var result = _context.Answers.AddAsync(answer); //ChangeTracking
                await _context.SaveChangesAsync();//MUST !!!!
                                                  //return result; //NOK	
                return answer;   //ByRef   -> autoIdentity ingevuld            
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.InnerException.Message);
                return null;
            }
        }
    }
}
