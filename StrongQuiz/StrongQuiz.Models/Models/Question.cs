using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StrongQuiz.Models.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionID { get; set; }
        [NotMapped]
        public string ImageUrl
        {
            get { return $"/images/{this.QuestionID}.jpg"; }
        }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name is limited to 50 characters in length.")]
        public string? QuestionName { get; set; }
        [NotMapped]
        public bool modify { get; set; } = false;
        public Guid QuizId { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
