using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrongQuiz.Models.ViewModel
{
    public class AddQuestionToQuiz_VM
    {
        public Guid QuizId { get; set; }
        public int QuestionCount { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name is limited to 50 characters in length.")]
        public string QuestionName { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
