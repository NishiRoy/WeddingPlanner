using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace weddingPlanner.Models
{
    public class Guest
    {
        public int Id{get;set;}
        public int UserId{get;set;}
        public User myUser{get;set;}

        public int WeddingId{get;set;}
        public Wedding weddings{get;set;}

        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}

        public Guest(){
            CreatedAt=DateTime.Now;
            UpdatedAt=DateTime.Now;
        }
    }
}