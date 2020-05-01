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
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name is limited to 50 characters in length.")]
        public string Name { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Name is limited to 50 characters in length.")]
        [MinLength(10,ErrorMessage = "minimum 10 characters long")]
        public string Description { get; set; }
        [NotMapped]
        public string ImageUrl
        {
            get { return $"/images/{this.Id}.jpg"; }
        }
        public enum Difficulties
        {
            beginner=0,
            advanced=1,
            professional=2
        }
        public Difficulties Difficulty { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Maximum length is 250 characters.")]
        public string Below50Quote { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Maximum length is 250 characters.")]
        public string Below75Quote { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Maximum length is 250 characters.")]
        public string Above75Quote { get; set; }
        public int QuestionCount { get; set; }
        public virtual IList<Question> Questions { get; set; }
        public IList<ScoreQuiz> scoreQuizzes { get; set; } = new List<ScoreQuiz>();
    }
}
