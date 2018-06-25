using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace weddingPlanner.Models
{
    public class Wedding{
        public int Id{get;set;}
        [Required]
        public string wedderOne{get;set;}

        [Required]
        public string wedderTwo{get;set;}

        [Required]
        public string Address{get;set;}

        [Required]
        public DateTime wedddingDate{get;set;}
        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}

        public int UserId{get;set;}
        public User myuser{get;set;}
        public List<Guest> guests{get;set;}

        public Wedding(){
            guests=new List<Guest>();
            CreatedAt=DateTime.Now;
            UpdatedAt=DateTime.Now;
        }

    }

}