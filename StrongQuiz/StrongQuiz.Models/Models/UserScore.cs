using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrongQuiz.Models.Models
{
    public class UserScore
    {
        [Key]
        public int Id { get; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public Guid QuizId { get; set; }
        public string ApplicationUserId { get; set; }
        public Quiz Quiz { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
