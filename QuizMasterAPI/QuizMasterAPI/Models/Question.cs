using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuizMasterAPI.Models
{
    public class Question
    {
        [Key]
        [JsonProperty("qId")]
        public int QId { get; set; }
        [StringLength(250),Required, Index(IsUnique = true)]
        public String Ques { get; set; }
        [MaxLength(250), Required]
        public String  OptionOne { get; set; }
        [Required]
        public String OptionTwo { get; set; }
        [Required]
        public String OptionThree { get; set; }
        [Required]
        public String OptionFour { get; set; }
        [Required]
        public String Answer { get; set; }

    }
}