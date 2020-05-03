using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongQuiz.API.Models
{
    public class Question_DTO
    {
        public string QuestionName { get; set; }
        public virtual IList<Answers_DTO> Answers { get; set; }

    }
}
