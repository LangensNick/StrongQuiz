using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongQuiz.API.Models
{
    public class Quiz_DTO
    {
        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [StringLength(250, MinimumLength = 2, ErrorMessage = "Only 250 characters allowed.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "moeilijkheidsgraad verplicht")]
        public string Difficulty { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Only 250 characters allowed.")]
        public string Below50Quote { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Only 250 characters allowed.")]
        public string Below75Quote { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Only 250 characters allowed.")]
        public string Above75Quote { get; set; }

        //public Dictionary<string, string> Questions = new Dictionary<string, string>();
        [JsonProperty("Questions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Question_DTO> Questions {get;set;} = new List<Question_DTO>();
    }
}
