using Microsoft.EntityFrameworkCore;
using StrongQuiz.Models.Models;
using StrongQuiz.Web.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly StrongQuizDbContext _context;

        public QuestionRepository(StrongQuizDbContext strongQuizDbContext)
        {
            this._context = strongQuizDbContext;
        }
        public async Task<IEnumerable<Question>> GetQuestionsAsync() => await _context.Questions.ToListAsync<Question>();
        public async Task<Question> GetQuestionsAndAnswersAsync(Guid key) => await _context.Questions.Include(p => p.Answers).FirstOrDefaultAsync<Question>(p => p.QuestionID == key);
        public async Task<Question> GetQuestionByIdAsync(Guid Key) => await _context.Questions.FindAsync(Key);
        public async Task<Question> Add(Question question)
        {
            try
            {
                var result = _context.Questions.AddAsync(question); //ChangeTracking
                await _context.SaveChangesAsync();//MUST !!!!
                                                  //return result; //NOK	
                return question;   //ByRef   -> autoIdentity ingevuld            
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.InnerException.Message);
                return null;
            }
        }
    }
}
