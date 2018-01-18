using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuizMasterAPI.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        [MaxLength(50), Required, Index(IsUnique =true)]
        public String TeamName { get; set; }
        public String TeamMembers { get; set; }
        [Required]
        public int TeamScore { get; set; }
        public int QuestionsAttempted { get; set; }
        public int QuestionsPassed { get; set; }
        public int AnswersCorrect { get; set; }
    }
}