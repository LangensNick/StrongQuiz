using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongQuiz.API.Models
{
    public static class Mapper
    {
        public static Quiz_DTO ConvertTo_DTO(Quiz quiz, ref Quiz_DTO quiz_DTO)
        {

            //controleer objecten op null
            quiz_DTO.Above75Quote = quiz.Above75Quote;
            quiz_DTO.Below50Quote = quiz.Below50Quote;
            quiz_DTO.Below75Quote = quiz.Below75Quote;
            quiz_DTO.Difficulty = quiz.Difficulty.ToString();
            quiz_DTO.Name = quiz.Name;
            quiz_DTO.Description = quiz.Description;
            List<List<string>> questions = new List<List<string>>();
            foreach(var item in quiz.Questions)
            {
                List<string> question = new List<string>();
                question.Add(item.QuestionName);
                List<string> answers = new List<string>();
                foreach(var itemAnswer in item.Answers)
                {
                    List<string> answer = new List<string>();
                    answer.Add(itemAnswer.AnswerName);
                    if(itemAnswer.Correct == Answer.State.correct)
                    {
                        answer.Add("correct");
                    }
                   // answers.Add(answer);
                }

            }
 //           quiz_DTO.Questions = questions;

            /*//uitvlakken van navigatieproperties + controleren op null
            if (edu.Roles != null && edu.Roles.Count > 0)
            {
                foreach (var role in edu.Roles)
                {
                    if (role.UserRoles != null && role.UserRoles.Count > 0)
                    //indien niet included in SQL response of leeg
                    {
                        foreach (var pe in role.UserRoles)
                        {

                            //object ref not set error ->edu properties default aanmaken (new List)
                            edu_DTO.Users.Add(pe.User.UserName);
                        }
                    }
                }
            }
            else
            {
                // nullvalue handling gedrag bij Count == 0
                edu_DTO.Users = null;

            }
            */
            return quiz_DTO;
        }
    }
}
