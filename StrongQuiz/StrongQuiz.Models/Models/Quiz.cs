using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StrongQuiz.Models.Models
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name is limited to 50 characters in length.")]
        public string Name { get; set; }

        [StringLength(250, MinimumLength = 2, ErrorMessage = "Name is limited to 50 characters in length.")]
        public string Description { get; set; }
        [NotMapped]
        public string ImageUrl
        {
            get { return $"/images/{this.Name}.jpg"; }
        }
        public enum Difficulties
        {
            beginner=0,
            advanced=1,
            professional=2
        }
        public Difficulties Difficulty { get; set; }
        public string Below50Quote { get; set; }
        public string Below75Quote { get; set; }
        public string Above75Quote { get; set; }
        public int QuestionCount { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public ICollection<ScoreQuiz> scoreQuizzes { get; set; } = new List<ScoreQuiz>();
    }
}
