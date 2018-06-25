using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace weddingPlanner.Models
{
    public class User{
        public int Id{get;set;}

        [Required(ErrorMessage="First name is required")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$",ErrorMessage="Only Alphabets Required")]
        public string FirstName{get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$",ErrorMessage="Only Alphabets Required")]
        public string LastName{get;set;}

        [Required]
        [EmailAddress]
        public string Email{get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password{get;set;}

        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}

        List<Wedding> wedding{get;set;}
        List<Guest> guests{get;set;}

        public User(){
            wedding =new List<Wedding>();
            guests =new List<Guest>();
            CreatedAt=DateTime.Now;
            UpdatedAt=DateTime.Now;
        }
    }
}