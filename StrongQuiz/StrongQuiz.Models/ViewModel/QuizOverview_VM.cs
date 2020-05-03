using System;
using System.Collections.Generic;
using System.Text;

namespace StrongQuiz.Models.ViewModel
{
    public class QuizOverview_VM
    {
        public Guid QuizId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string ImageUrl
        {
            get { return $"/images/{this.QuizId}.jpg"; }
        }
        public bool QuizzedTaken { get; set; }
    }
}
