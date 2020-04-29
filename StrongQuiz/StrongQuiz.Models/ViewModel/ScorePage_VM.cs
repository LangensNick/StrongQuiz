using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrongQuiz.Models.ViewModel
{
    public class ScorePage_VM
    {
        public Guid quizId { get; set; }
        public string QuizImage { get; set; }
        public string UserName { get; set; }
        public int MaxScore { get; set; }
        public int Score { get; set; }

        public string Comment { get; set; }
        public ICollection<Question> questions { get; set; }
    }
}
