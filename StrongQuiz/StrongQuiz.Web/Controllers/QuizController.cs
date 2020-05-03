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
using RestSharp;
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
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var participated = await userScoreRepo.GetUserQuizzesAsync(user.Id);
            List<Guid> pp = new List<Guid>();

            foreach (var quizTaken in participated)
            {
                pp.Add(quizTaken.QuizId);
            }
            List<QuizOverview_VM> quizOverview = new List<QuizOverview_VM>();
            foreach(var quiz in Quizzes)
            {
                QuizOverview_VM quizOverview_VM = new QuizOverview_VM()
                {
                    QuizId = quiz.Id,
                    Description = quiz.Description,
                    Name = quiz.Name,
                    QuizzedTaken = pp.Contains(quiz.Id),
                    Difficulty = quiz.Difficulty.ToString()
                };
                quizOverview.Add(quizOverview_VM);
            }
            return View(quizOverview);
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Participate(string QuizId)
        {
            Quiz quiz = await quizRepository.GetQuizByIdAsync(Guid.Parse(QuizId));
            if(quiz == null)
            {
                return Redirect("/Error/404");
            }
            return View(quiz);
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Leaderboard(string QuizId)
        {
            try
            {
                IEnumerable<UserScore> leaderboard = await userScoreRepo.GetUserScoreWithUserAsync(Guid.Parse(QuizId));
                if(leaderboard == null || leaderboard.Count() == 0)
                {
                    return Redirect("/Error/400");
                }
                List<Leaderboard_VM> leaderboard_VMs = new List<Leaderboard_VM>();

                foreach (var item in leaderboard)
                {
                    var users = userManager.Users.Where(x => x.Id == item.ApplicationUserId);
                    var username = "";
                    foreach (var user in users)
                    {
                        username = user.UserName;
                    }
                    Leaderboard_VM leaderboard_VM = new Leaderboard_VM()
                    {
                        Username = username,
                        Score = item.Score,
                        MaxScore = item.MaxScore
                    };
                    leaderboard_VMs.Add(leaderboard_VM);

                }
                return View("Leaderboard", leaderboard_VMs);
            }
            catch (Exception ex)
            {
                return Redirect("/Error/400");
                throw;
            }
            
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> StartQuiz(string QuizId)
        {
            try
            {
                Guid quizGuid = Guid.Parse(QuizId);
                Quiz quiz = await quizRepository.GetQuizQuestionsAnswersAsync(quizGuid);
                if (quiz == null)
                {
                    return Redirect("/Error/404");
                }
                foreach (var question in quiz.Questions)
                {
                    question.modify = true;
                }

                return View(quiz);
            }
            catch (Exception)
            {
                return Redirect("/Error/400");
                throw;
            }
            
        }
        

        // POST: Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateQuizAsync(Quiz quiz, IFormCollection collection, IFormFile image1)
        {
            if (ModelState.IsValid)
            {
                try
            {
                Guid QuizQuid = Guid.NewGuid();
                var uploadPath = Path.Combine(he.WebRootPath, "images");
                    HttpCookie StudentCookies = new HttpCookie() { Comment = "halloooootjes" };
                if(image1 != null)
                {
                    if (image1.Length > 0)
                    {
                        var filePath = Path.Combine(uploadPath, QuizQuid.ToString() + ".jpg");
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image1.CopyToAsync(fileStream);
                        }
                    }
                }

                        quiz.Id = QuizQuid;

                IList<Question> questions = new List<Question>();
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
                AddQuestionsWithQuiz addQuestionsWithQuiz = new AddQuestionsWithQuiz() { quiz = quiz, questions = questions };
                return View("AddQuestionToQuiz", addQuestionsWithQuiz);
            }
            catch (Exception exc)
            {
                return View();
            }
        }
            else
            {
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuestionToQuiz(Quiz quiz,ICollection<Question> questions, IFormCollection collection, List<IFormFile> questionImage,List<string> imageav)
        {
            try
            {
                int imageCounter = 0;
                int i = 0;
                var uploadPath = Path.Combine(he.WebRootPath, "images");
                await quizRepository.Add(quiz);

                foreach (var question in questions)
                {
                    await questionRepository.Add(question);
                    Guid QuestionGuid = Guid.NewGuid();
                    if (Int16.Parse(imageav[i]) == 1)
                    {
                        if (questionImage[imageCounter].Length > 0)
                        {
                            var filePath = Path.Combine(uploadPath, question.QuestionID + ".jpg");
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await questionImage[imageCounter].CopyToAsync(fileStream);
                            }
                        }
                        imageCounter++;

                    }
                    i++;
                }
                ICollection<Quiz> quizzes = await quizRepository.GetQuizzesAsync();
                var Quizzes = await quizRepository.GetQuizzesAsync();
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var participated = await userScoreRepo.GetUserQuizzesAsync(user.Id);
                List<Guid> pp = new List<Guid>();

                foreach (var quizTaken in participated)
                {
                    pp.Add(quizTaken.QuizId);
                }
                List<QuizOverview_VM> quizOverview = new List<QuizOverview_VM>();
                foreach (var quizs in Quizzes)
                {
                    QuizOverview_VM quizOverview_VM = new QuizOverview_VM()
                    {
                        QuizId = quizs.Id,
                        Description = quizs.Description,
                        Name = quizs.Name,
                        QuizzedTaken = pp.Contains(quizs.Id),
                        Difficulty = quiz.Difficulty.ToString()
                    };
                    quizOverview.Add(quizOverview_VM);
                }
                return View("MyQuiz", quizOverview);
            }
            catch (Exception exc)
            {
                var Quizzes = await quizRepository.GetQuizzesAsync();
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var participated = await userScoreRepo.GetUserQuizzesAsync(user.Id);
                List<Guid> pp = new List<Guid>();

                foreach (var quizTaken in participated)
                {
                    pp.Add(quizTaken.QuizId);
                }
                List<QuizOverview_VM> quizOverview = new List<QuizOverview_VM>();
                foreach (var quizs in Quizzes)
                {
                    QuizOverview_VM quizOverview_VM = new QuizOverview_VM()
                    {
                        QuizId = quizs.Id,
                        Description = quizs.Description,
                        Name = quizs.Name,
                        QuizzedTaken = pp.Contains(quizs.Id)
                    };
                    quizOverview.Add(quizOverview_VM);
                }
                return View("MyQuiz", quizOverview);
            }
        }
        // GET: Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitQuiz(IFormCollection collection)
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
                var answer = collection[question].ToString();
                ILookup<bool, Answer> CorrectAnswers = questionForCorrection.Answers.ToLookup(p => p.Correct == Answer.State.correct);
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