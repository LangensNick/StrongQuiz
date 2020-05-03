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
            List<Question_DTO> questions = new List<Question_DTO>();
            foreach(var question in quiz.Questions)
            {
                Question_DTO question_DTO = new Question_DTO();
                List<Answers_DTO> answers_DTOs = new List<Answers_DTO>();
                foreach(var answer in question.Answers)
                {
                    Answers_DTO answers_DTO = new Answers_DTO()
                    {
                        AnswerName = answer.AnswerName,
                        Correct = answer.Correct.ToString()
                    };
                    answers_DTOs.Add(answers_DTO);
                }
                question_DTO.Answers = answers_DTOs;
                question_DTO.QuestionName = question.QuestionName;
                questions.Add(question_DTO);
            }
            quiz_DTO.Questions = questions;
            return quiz_DTO;
        }
        public static Question_DTO ConvertTo_DTO(Question question, ref Question_DTO question_DTO)
        {
            question_DTO.QuestionName = question.QuestionName;
            List<Answers_DTO> answers_DTOs = new List<Answers_DTO>();
            foreach(var answer in question.Answers)
            {
                Answers_DTO answers_DTO = new Answers_DTO();
                answers_DTO.AnswerName = answer.AnswerName;
                answers_DTO.Correct = answer.Correct.ToString();
                answers_DTOs.Add(answers_DTO);
            }
            question_DTO.Answers = answers_DTOs;
            return question_DTO;
        }
        public static List<Answers_DTO> ConvertTo_DTO(IEnumerable<Answer> answers, ref List<Answers_DTO> answer_DTO)
        {
            foreach(var answer in answers)
            {
                Answers_DTO answerinstance = new Answers_DTO();
                answerinstance.AnswerName = answer.AnswerName;
                answerinstance.Correct = answer.Correct.ToString();
                answer_DTO.Add(answerinstance);
            }

            return answer_DTO;
        }

    }
}
