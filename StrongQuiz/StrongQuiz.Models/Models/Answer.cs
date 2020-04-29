using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StrongQuiz.Models.Models
{
    public class Answer
    {
        [Key]
        public Guid AnswerId { get; set; }
        public string AnswerName { get; set; }
        public enum State
        {
            correct = 1,
            incorrect = 0
        }
        [NotMapped]
        public bool selected { get; set; }

        [NotMapped]
        public string ImageUrl
        {
            get { return $"./images/Answers/{this.AnswerName}.jpg"; }
        }
        public State? Correct { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
