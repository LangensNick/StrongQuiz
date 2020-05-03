using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrongQuiz.Models.ViewModel
{
    public class AddQuestionsWithQuiz
    {
        public Quiz quiz { get; set; }
        public IList<Question> questions { get; set; }
    }
}
