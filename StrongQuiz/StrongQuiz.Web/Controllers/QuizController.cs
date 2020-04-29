using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using StrongQuiz.Models.Models;
using StrongQuiz.Models.Repositories;
using StrongQuiz.Models.ViewModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace StrongQuiz.Web.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository quizRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment he;
        private readonly IUserScoreRepositories userScoreRepo;

        public QuizController(IQuizRepository quizRepository, IAnswerRepository answerRepository, IQuestionRepository questionRepository, UserManager<ApplicationUser> userManager
            , IHostingEnvironment e, IUserScoreRepositories userScoreRepo)
        {
            this.quizRepository = quizRepository;
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
            this.userManager = userManager;
            this.he = e;
            this.userScoreRepo = userScoreRepo;
            //this.scoreQuizRepository = scoreQuizRepository;
        }

        // GET: Quiz
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateQuiz() => View();
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> MyQuiz()
        {
            var Quizzes = await quizRepository.GetQuizzesAsync();
            return View(Quizzes);
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Participate(string QuizId)
        {
            Quiz quiz = await quizRepository.GetQuizByIdAsync(Guid.Parse(QuizId));
            return View(quiz);
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Leaderboard(string QuizId)
        {
            var leaderboard = await userScoreRepo.GetUserScoreWithUserAsync(Guid.Parse(QuizId));
            return View("Leaderboard", leaderboard);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> StartQuiz(string QuizId)
        {
            Quiz quiz = await quizRepository.GetQuizQuestionsAnswersAsync(Guid.Parse(QuizId));
            foreach(var question in quiz.Questions)
            {
                question.modify = true;
            }

            return View(quiz);
        }
        // GET: Quiz/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Quiz/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateQuizAsync(IFormCollection collection, IFormFile image1)
        {
            try
            {
                var uploadPath = Path.Combine(he.WebRootPath, "images");
                if (image1.Length > 0)
                {
                    var filePath = Path.Combine(uploadPath, collection["Name"] + ".jpg");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image1.CopyToAsync(fileStream);
                    }
                }
                // TODO: Add insert logic here
                Quiz quiz = new Quiz() {Id = Guid.NewGuid(), Name = collection["Name"], Description = collection["Description"], QuestionCount = Int32.Parse(collection["QuestionCount"]), Above75Quote = collection["Above75Quote"], Below75Quote = collection["Below75Quote"], Below50Quote = collection["Below50Quote"] };
                switch (Int16.Parse(collection["Difficulty"]))
                {
                    case 0:
                        quiz.Difficulty = Quiz.Difficulties.beginner;
                        break;
                    case 1:
                        quiz.Difficulty = Quiz.Difficulties.advanced;
                        break;
                    case 2:
                        quiz.Difficulty = Quiz.Difficulties.professional;
                        break;
                    default:
                        break;
                }
                await quizRepository.Add(quiz);
                ICollection<Question> questions = new List<Question>();
                for (var i = 0; i < quiz.QuestionCount; i++)
                {
                    List<Answer> answers = new List<Answer>();

                    for (var j = 0; j < 4; j++)
                    {
                        Answer answer = new Answer() { AnswerId = Guid.NewGuid() };
                        answers.Add(answer);
                    }
                    Question question1 = new Question() { QuestionID = Guid.NewGuid(), Answers = answers, Quiz = quiz };

                    questions.Add(question1);
                }
                ViewBag.QuizId = quiz.Id;
                return View("AddQuestionToQuiz", questions);
            }
            catch (Exception exc)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuestionToQuiz(IFormCollection collection, List<IFormFile> questionImage,List<string> imageav)
        {
            try
            {
                var uploadPath = Path.Combine(he.WebRootPath, "images");
                int imageCounter = 0;
                // TODO: Add insert logic here
                for (var i = 0; i < collection["name"].Count(); i++)
                {
                    List<Answer> answers = new List<Answer>();
                    for (var j = 0; j < 4; j++)
                    {
                        var name = collection["item.QuestionName"];
                        int index = j + (i * 4);
                        if (Int32.Parse(collection["answer.Correct"][index]) == 1)
                        {
                            Answer newanswer = new Answer() { AnswerId = Guid.NewGuid(), AnswerName = collection["answer.AnswerName"][index], Correct = Answer.State.correct };
                            answers.Add(newanswer);
                        }
                        else
                        {
                            Answer newanswer = new Answer() { AnswerId = Guid.NewGuid(), AnswerName = collection["answer.AnswerName"][index], Correct = Answer.State.incorrect };
                            answers.Add(newanswer);
                        }
                    }
                    var value = collection["name"][0];
                    Guid QuestionGuid = Guid.NewGuid();
                    if (Int16.Parse(imageav[i]) == 1)
                    {
                        if (questionImage[imageCounter].Length > 0)
                        {
                            var filePath = Path.Combine(uploadPath, QuestionGuid + ".jpg");
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await questionImage[imageCounter].CopyToAsync(fileStream);
                            }
                        }
                        imageCounter++;

                    }

                        Question newquestion = new Question() { QuestionID = QuestionGuid, QuestionName = collection["item.QuestionName"][i], Answers = answers, QuizId = Guid.Parse(value) };
                    await questionRepository.Add(newquestion);



                }
                
                var quizzes = await quizRepository.GetQuizzesAsync();
                return View("MyQuiz",quizzes);
            }
            catch (Exception exc)
            {
                return View("MyQuiz");
            }
        }
        // GET: Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitQuiz(IFormCollection collection, ICollection<Question> questions)
        {
            var user = userManager.FindByNameAsync(User.Identity.Name).Result;
            List<Question> Correction = new List<Question>();
            Quiz quiz = await quizRepository.GetQuizByIdAsync(Guid.Parse(collection["Id"]));
            var TotalScore = collection["question.QuestionID"].Count();
            var Score = 0;
            foreach (var question in collection["question.QuestionID"])
            {
                Question questionForCorrection = await questionRepository.GetQuestionsAndAnswersAsync(Guid.Parse(question));
                questionForCorrection.modify = false;
                var questionid = question;
                var question1 = await answerRepository.GetAnswersByQuestionAsync(Guid.Parse(questionid));
                var answer = collection[questionid].ToString();
                ILookup<bool, Answer> CorrectAnswers = question1.ToLookup(p => p.Correct == Answer.State.correct);
                foreach (var correctanswer in CorrectAnswers[true])
                {
                    if (correctanswer.AnswerName == answer)
                    {
                        Score++;

                    }
                    
                }
                foreach(var possibleAnswers in questionForCorrection.Answers)
                {
                    if(possibleAnswers.AnswerName == answer)
                    {
                        possibleAnswers.selected = true;
                    }
                }
                Correction.Add(questionForCorrection);

            }
            try
            {
                UserScore userScore = new UserScore() { ApplicationUserId = user.Id, QuizId = quiz.Id, MaxScore = TotalScore, Score = Score, ApplicationUser = user, Quiz = quiz };
                await userScoreRepo.Add(userScore);
                double reCalcScore = ((double)Score / (double)TotalScore) * 10;
                var comment = "";
                if (reCalcScore >= 7.5)
                {
                    comment = quiz.Above75Quote;
                }
                else if((reCalcScore < 7.5) & (reCalcScore > 5))
                {
                    comment = quiz.Below75Quote;

                }
                else
                {
                    comment = quiz.Below50Quote;

                }
                ScorePage_VM scorePage_VM = new ScorePage_VM() { MaxScore = TotalScore, Score = Score, Comment = comment, questions= Correction, quizId = quiz.Id };

                return View("ScorePage", scorePage_VM);
            }
            catch (Exception exc)
            {
                return View("MyQuiz");
            }
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Quiz/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Quiz/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}