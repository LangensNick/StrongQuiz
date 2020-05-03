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
    public class QuizRepository : IQuizRepository
    {
        private readonly StrongQuizDbContext _context;

        public QuizRepository(StrongQuizDbContext strongQuizDbContext)
        {
            this._context = strongQuizDbContext;
        }
        public async Task<ICollection<Quiz>> GetQuizzesAsync() => await _context.Quizzes.ToListAsync<Quiz>();

        public async Task<Quiz> GetQuizByIdAsync(Guid Key) => await _context.Quizzes.FindAsync(Key);
        public async Task<Quiz> GetQuizQuestionsAnswersAsync(Guid Key)
        {
            Quiz quiz = await _context.Quizzes.Include(p => p.Questions).ThenInclude(it => it.Answers).SingleOrDefaultAsync<Quiz>(p => p.Id == Key);

            return quiz;
        }
        public async Task<Quiz> Add(Quiz quiz)
        {
            try
            {
                var result = _context.Quizzes.AddAsync(quiz); //ChangeTracking
                await _context.SaveChangesAsync();//MUST !!!!
                                                 //return result; //NOK	
                return quiz;   //ByRef   -> autoIdentity ingevuld            
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.InnerException.Message);
                return null;
            }
        }
        

    }
}
