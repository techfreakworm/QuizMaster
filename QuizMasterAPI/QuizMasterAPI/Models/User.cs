using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuizMasterAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(50),Required, Index(IsUnique =true)]
        public String UserName { get; set; }
        [Required]
        public String UserType { get; set; }
        [Required]
        public String UserPass { get; set; }

    }
}