using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrongQuiz.Models.Repositories;
using StrongQuiz.API.Models;
using StrongQuiz.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StrongQuiz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IUserScoreRepositories userScoreRepo;
        private readonly IQuizRepository quizRepo;
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + ",Identity.Application";
        public QuizController(IUserScoreRepositories userScoreRepositories, IQuizRepository quizRepository)
        {
            this.userScoreRepo = userScoreRepositories;
            this.quizRepo = quizRepository;
        }
        // GET: api/Quiz
        [HttpGet("taken/{guid}", Name = "GetTimesTakenByQuiz")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<int> GetTimesTakenByQuiz(string guid)
        {
            int result = await userScoreRepo.GetTimesTakenForEachQuiz(Guid.Parse(guid));
            return result;
        }

        [HttpGet("{guid}", Name = "QuizById")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<Quiz_DTO> QuizById(string guid)
        {
            Quiz_DTO quiz_DTO = new Quiz_DTO();
            Quiz quiz = await quizRepo.GetQuizQuestionsAnswersAsync(Guid.Parse(guid));
            quiz_DTO = Mapper.ConvertTo_DTO(quiz, ref quiz_DTO);
            return quiz_DTO;
        }

       

        // POST: api/Quiz
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Quiz/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
