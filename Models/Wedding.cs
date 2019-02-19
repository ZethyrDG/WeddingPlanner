using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        public int WeddingId{get;set;}
        [Required]
        [MinLength(2, ErrorMessage="Name must be at least 2 characters!")]
        [Display(Name = "Wedder One:")]
        [NotMapped]
        public string Wedder1Name{get;set;}
        public int Wedder1Id{get;set;}
        public User Wedder1{get;set;}
        [Required]
        [MinLength(2, ErrorMessage="Name must be at least 2 characters!")]
        [Display(Name = "Wedder Two:")]
        [NotMapped]
        public string Wedder2Name{get;set;}
        public int Wedder2Id{get;set;}
        public User Wedder2{get;set;}
        [Required]
        [Display(Name = "Date:")]
        public DateTime Date{get;set;}
        [Required]
        [MinLength(2, ErrorMessage="Address must be at least 2 characters!")]
        [Display(Name = "Wedding Address:")]
        public string Address{get;set;}
        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;
        public List<Guests> GuestList{get;set;}
    }
}