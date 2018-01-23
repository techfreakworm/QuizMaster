using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace QuizMasterAPI.Models
{
    public class User
    {
        public User(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(json);
            UserName = (string)jsonObject["UserName"];
            UserPass= (string)jsonObject["UserPass"];
            UserType= (string)jsonObject["UserType"];
        }
        public User() { }
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