using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Guests
    {
        public int GuestsId{get;set;}
        public int UserId{get;set;}
        public User User{get;set;}
        public int WeddingId{get;set;}
        public Wedding Wedding{get;set;}
    }
}