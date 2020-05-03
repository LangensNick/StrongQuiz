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
        private readonly IQuestionRepository questionRepo;
        private readonly IAnswerRepository answerRepository;
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + ",Identity.Application";
        public QuizController(IUserScoreRepositories userScoreRepositories, IQuizRepository quizRepository, IQuestionRepository questionRepo
            , IAnswerRepository answerRepository)
        {
            this.userScoreRepo = userScoreRepositories;
            this.quizRepo = quizRepository;
            this.questionRepo = questionRepo;
            this.answerRepository = answerRepository;
        }
        // GET: api/Quiz
        [HttpGet("taken/{guid}", Name = "GetTimesTakenByQuiz")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<ActionResult<int>> GetTimesTakenByQuiz(string guid)
        {
            try
            {
                int result = await userScoreRepo.GetTimesTakenForEachQuiz(Guid.Parse(guid));
                if (result == null)
                {
                    return NotFound();
                }
                
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Wrong id");
                throw;
            }
            
        }
        [HttpGet("question/{guid}", Name = "QuestionById")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<ActionResult<Question_DTO>> QuestionById(string guid)
        {
            try
            {
                Question question = await questionRepo.GetQuestionsAndAnswersAsync(Guid.Parse(guid));
                Question_DTO question_DTO = new Question_DTO();
                if (question == null)
                {
                    return NotFound();
                }
                question_DTO = Mapper.ConvertTo_DTO(question, ref question_DTO);
                return question_DTO;
            }
            catch (Exception ex)
            {
                return BadRequest();
                throw;
            }

        }

        [HttpGet("{guid}", Name = "QuizById")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<ActionResult<Quiz_DTO>> QuizById(string guid)
        {
            try
            {
                Quiz_DTO quiz_DTO = new Quiz_DTO();
                Quiz quiz = await quizRepo.GetQuizQuestionsAnswersAsync(Guid.Parse(guid));
                if (quiz == null)
                {
                    return NotFound();
                }
                quiz_DTO = Mapper.ConvertTo_DTO(quiz, ref quiz_DTO);
                return Ok(quiz_DTO);
            }
            catch (Exception)
            {
                return BadRequest("Wrong id");
                throw;
            }
            
        }
        [HttpGet("answer/{guid}", Name = "AnswerById")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<ActionResult<Quiz_DTO>> AnswerById(string guid)
        {
            try
            {
                List<Answers_DTO> answer_DTO = new List<Answers_DTO>();
                IEnumerable<Answer> answers = await answerRepository.GetAnswersByQuestionAsync(Guid.Parse(guid));
                if (answers == null)
                {
                    return NotFound();
                }
                answer_DTO = Mapper.ConvertTo_DTO(answers, ref answer_DTO);
                return Ok(answer_DTO);
            }
            catch (Exception)
            {
                return BadRequest("Wrong id");
                throw;
            }

        }



    }
}
